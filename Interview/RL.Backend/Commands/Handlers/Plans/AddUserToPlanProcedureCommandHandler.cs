using MediatR;
using Microsoft.EntityFrameworkCore;
using RL.Backend.Exceptions;
using RL.Backend.Models;
using RL.Data;
using RL.Data.DataModels;

namespace RL.Backend.Commands.Handlers.Plans;

public class AddUserToPlanProcedureCommandHandler : IRequestHandler<AddUserToPlanProcedureCommand, ApiResponse<Unit>>
{
    private readonly RLContext _context;

    public AddUserToPlanProcedureCommandHandler(RLContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<Unit>> Handle(AddUserToPlanProcedureCommand request, CancellationToken cancellationToken)
    {
        try
        {
            //Validate request
            if (request.PlanId < 1)
                return ApiResponse<Unit>.Fail(new BadRequestException("Invalid PlanId"));
            if (request.ProcedureId < 1)
                return ApiResponse<Unit>.Fail(new BadRequestException("Invalid ProcedureId"));

            var plan = await _context.Plans
                .Include(p => p.PlanProcedures)
                .FirstOrDefaultAsync(p => p.PlanId == request.PlanId);
            var procedure = await _context.Procedures.FirstOrDefaultAsync(p => p.ProcedureId == request.ProcedureId);

            if (plan is null)
                return ApiResponse<Unit>.Fail(new NotFoundException($"PlanId: {request.PlanId} not found"));
            if (procedure is null)
                return ApiResponse<Unit>.Fail(new NotFoundException($"ProcedureId: {request.ProcedureId} not found"));
            if (plan.PlanProcedures.Any(p => p.ProcedureId == procedure.ProcedureId))
            {
                var planProcedure = plan.PlanProcedures.FirstOrDefault(p => p.ProcedureId == procedure.ProcedureId);
                List<Assignment> assignments = new List<Assignment>();
                var existingAssignments = _context.Assignments
                    .Where(a => a.PlanId == plan.PlanId && a.ProcedureId == procedure.ProcedureId)
                    .ToList();

                if (existingAssignments.Any())
                {
                    _context.Assignments.RemoveRange(existingAssignments);
                    planProcedure.Assignments = null;
                }
                if (request.UserId.Count > 0)
                {
                    foreach (var item in request.UserId)
                    {
                        assignments.Add(new Assignment
                        {
                            PlanId = plan.PlanId,
                            ProcedureId = procedure.ProcedureId,
                            UserId = item
                        });
                    }
                }
                planProcedure.Assignments = assignments;
            }

            Console.WriteLine($"Added procedure to plan : {plan.PlanId}");
            Console.WriteLine($"Added procedure : {procedure.ProcedureId}");
            await _context.SaveChangesAsync();

            return ApiResponse<Unit>.Succeed(new Unit());
        }
        catch (Exception e)
        {
            return ApiResponse<Unit>.Fail(e);
        }
    }
}
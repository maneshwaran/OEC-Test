import React, { useEffect, useState } from "react";
import ReactSelect from "react-select";
import {
    addUserToPlanProcedure
  } from "../../../api/api";
const PlanProcedureItem = ({ planId, procedure, users, assignedUsers }) => {
    const [selectedUsers, setSelectedUsers] = useState([]);

    useEffect(() => {
        let selectUser = []
        if(assignedUsers){
            selectUser = users.filter(u => 
                assignedUsers.some(au => au.userId === u.value)
            );
        }
        setSelectedUsers(selectUser);
        if(procedure.procedureId >4){
            console.log(assignedUsers);
            console.log(selectedUsers);
        }
        
    },[])
    const handleAssignUserToProcedure = async (e) => {

        let users = e.map(user => user.value);
        await addUserToPlanProcedure(planId, procedure.procedureId, users);
        // TODO: Remove console.log and add missing logic
        //console.log(e);
        setSelectedUsers(e);
    };

    return (
        <div className="py-2">
            <div>
                {procedure.procedureTitle}
            </div>

            <ReactSelect
                className="mt-2"
                placeholder="Select User to Assign"
                isMulti={true}
                options={users}
                value={selectedUsers}
                onChange={(e) => handleAssignUserToProcedure(e)}
            />
        </div>
    );
};

export default PlanProcedureItem;

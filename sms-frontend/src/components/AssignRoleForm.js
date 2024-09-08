import React, { useState, useEffect } from 'react';
import axios from 'axios';

const AssignRoleForm = () => {
    const [users, setUsers] = useState([]);
    const [selectedUser, setSelectedUser] = useState(null);
    const [entityId, setEntityId] = useState('');

    useEffect(() => {
        axios.get('/api/users').then(response => setUsers(response.data));
    }, []);

    const handleSubmit = () => {
        axios.post(`/api/users/${selectedUser}/assign`, { entityId })
            .then(response => {
                alert('User assigned successfully');
            })
            .catch(error => {
                console.error('There was an error assigning the user!', error);
            });
    };

    return (
        <div>
            <h1>Assign User to Entity</h1>
            <select onChange={e => setSelectedUser(e.target.value)}>
                <option value="">Select User</option>
                {users.map(user => (
                    <option key={user.userId} value={user.userId}>
                        {user.username}
                    </option>
                ))}
            </select>
            <input
                type="text"
                placeholder="Entity ID"
                value={entityId}
                onChange={e => setEntityId(e.target.value)}
            />
            <button onClick={handleSubmit}>Assign User</button>
        </div>
    );
};

export default AssignRoleForm;
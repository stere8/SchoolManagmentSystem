import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Form, Button, Alert } from 'react-bootstrap';
import { BASE_URL } from '../settings';

const UserAssignment = () => {
    const [users, setUsers] = useState([]);
    const [entities, setEntities] = useState([]); // Separate state for entities
    const [selectedUser, setSelectedUser] = useState('');
    const [selectedEntity, setSelectedEntity] = useState('');
    const [role, setRole] = useState(''); // Dropdown to select role
    const [message, setMessage] = useState('');

    useEffect(() => {
        if (role) {
            fetchUsersByRole(role); // Fetch users based on selected role
        }
    }, [role]);

    const fetchUsersByRole = async (role) => {
        let endpoint = '';
        // Use different endpoints for Students and Staff
        if (role === 'student') {
            endpoint = '/Students';
        } else if (role === 'teacher') {
            endpoint = '/Staff';
        }

        if (endpoint) {
            try {
                const response = await axios.get(`${BASE_URL}${endpoint}`);
                setUsers(response.data); // Setting users based on role
            } catch (error) {
                console.error(`Error fetching ${role}s:`, error);
            }
        }

        // Fetch entities
        try {
            const response = await axios.get(`${BASE_URL}/Users/role/${role}`);
            setEntities(response.data); // Setting entities
        } catch (error) {
            console.error('Error fetching entities:', error);
        }
    };

    const handleUserChange = (userId) => {
        setSelectedUser(userId);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const response = await axios.post(`${BASE_URL}/users/assign`, {
                userId: selectedUser,
                entityId: selectedEntity
            });
            setMessage('User assigned successfully');
        } catch (error) {
            setMessage('Error assigning user: ' + error.response.data);
        }
    };

    return (
        <div>
            <h1>Assign User to Entity</h1>
            {message && <Alert variant="info">{message}</Alert>}
            <Form onSubmit={handleSubmit}>

                {/* Dropdown to select role (Teacher or Student) */}
                <Form.Group controlId="formRole">
                    <Form.Label>Select Role</Form.Label>
                    <Form.Control as="select" value={role} onChange={e => setRole(e.target.value)}>
                        <option value="">Select Role</option>
                        <option value="teacher">Teacher</option>
                        <option value="student">Student</option>
                    </Form.Control>
                </Form.Group>

                {/* Dropdown to select User */}
                <Form.Group controlId="formUser">
                    <Form.Label>Select User</Form.Label>
                    <Form.Control as="select" value={selectedUser} onChange={e => handleUserChange(e.target.value)}>
                        <option value="">Select User</option>
                        {users.map(user => (
                            <option
                                key={role === 'student' ? user.studentId : user.staffId}
                                value={role === 'student' ? user.studentId : user.staffId}
                            >
                                {user.firstName} {user.lastName}
                            </option>
                        ))}
                    </Form.Control>
                </Form.Group>

                {/* Dropdown to select Entity based on selected user */}
                <Form.Group controlId="formEntity">
                    <Form.Label>Select Entity</Form.Label>
                    <Form.Control as="select" value={selectedEntity} onChange={e => setSelectedEntity(e.target.value)}>
                        <option value="">Select Entity</option>
                        {entities.map(entity => (
                            <option key={entity.id} value={entity.id}>
                                {entity.userName}
                            </option>
                        ))}
                    </Form.Control>
                </Form.Group>

                <Button variant="primary" type="submit">
                    Assign User
                </Button>
            </Form>
        </div>
    );
};

export default UserAssignment;

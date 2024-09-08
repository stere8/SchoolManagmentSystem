// /sms-frontend/src/components/UserAssignment.js
import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Form, Button, Alert } from 'react-bootstrap';
import { BASE_URL } from '../settings';

const UserAssignment = () => {
    const [users, setUsers] = useState([]);
    const [teachers, setTeachers] = useState([]);
    const [students, setStudents] = useState([]);
    const [selectedUser, setSelectedUser] = useState('');
    const [selectedEntity, setSelectedEntity] = useState('');
    const [message, setMessage] = useState('');

    useEffect(() => {
        fetchUsers();
        fetchTeachers();
        fetchStudents();
    }, []);

    const fetchUsers = async () => {
        try {
            const response = await axios.get(`${BASE_URL}/users`);
            setUsers(response.data);
        } catch (error) {
            console.error('Error fetching users:', error);
        }
    };

    const fetchTeachers = async () => {
        try {
            const response = await axios.get(`${BASE_URL}/teachers`);
            setTeachers(response.data);
        } catch (error) {
            console.error('Error fetching teachers:', error);
        }
    };

    const fetchStudents = async () => {
        try {
            const response = await axios.get(`${BASE_URL}/students`);
            setStudents(response.data);
        } catch (error) {
            console.error('Error fetching students:', error);
        }
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
                <Form.Group controlId="formUser">
                    <Form.Label>Select User</Form.Label>
                    <Form.Control as="select" value={selectedUser} onChange={e => setSelectedUser(e.target.value)}>
                        <option value="">Select User</option>
                        {users.map(user => (
                            <option key={user.userId} value={user.userId}>
                                {user.username}
                            </option>
                        ))}
                    </Form.Control>
                </Form.Group>
                <Form.Group controlId="formEntity">
                    <Form.Label>Select Entity</Form.Label>
                    <Form.Control as="select" value={selectedEntity} onChange={e => setSelectedEntity(e.target.value)}>
                        <option value="">Select Entity</option>
                        {selectedUser && users.find(user => user.userId === selectedUser).role === 'Teacher' && teachers.map(teacher => (
                            <option key={teacher.teacherId} value={teacher.teacherId}>
                                {teacher.firstName} {teacher.lastName}
                            </option>
                        ))}
                        {selectedUser && users.find(user => user.userId === selectedUser).role === 'Student' && students.map(student => (
                            <option key={student.studentId} value={student.studentId}>
                                {student.firstName} {student.lastName}
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
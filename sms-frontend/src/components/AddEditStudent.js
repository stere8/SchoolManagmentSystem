import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate, useParams } from 'react-router-dom';
import { BASE_URL } from '../settings';
import { Form, Button, Container } from 'react-bootstrap';

const AddEditStudent = () => {
    const [student, setStudent] = useState({ firstName: '', lastName: '', dateOfBirth: '' });
    const navigate = useNavigate();
    const { id } = useParams();

    useEffect(() => {
        if (id) {
            axios.get(`${BASE_URL}/students/${id}`)
                .then(response => {
                    const studentData = response.data;
                    studentData.dateOfBirth = studentData.dateOfBirth.split('T')[0]; // Format date as yyyy-mm-dd
                    setStudent(studentData);
                })
                .catch(error => console.error('Error fetching student data:', error));
        }
    }, [id]);

    const handleChange = e => {
        const { name, value } = e.target;
        setStudent(prevState => ({ ...prevState, [name]: value }));
    };

    const handleSubmit = e => {
        e.preventDefault();
        if (id) {
            axios.put(`${BASE_URL}/students/${id}`, student)
                .then(() => navigate('/students'))
                .catch(error => console.error('Error updating student:', error));
        } else {
            axios.post(`${BASE_URL}/students`, student)
                .then(() => navigate('/students'))
                .catch(error => console.error('Error adding student:', error));
        }
    };

    return (
        <Container>
            <h1>{id ? 'Edit Student' : 'Add Student'}</h1>
            <Form onSubmit={handleSubmit}>
                <Form.Group controlId="firstName">
                    <Form.Label>First Name</Form.Label>
                    <Form.Control
                        type="text"
                        name="firstName"
                        value={student.firstName}
                        onChange={handleChange}
                        required
                    />
                </Form.Group>
                <Form.Group controlId="lastName">
                    <Form.Label>Last Name</Form.Label>
                    <Form.Control
                        type="text"
                        name="lastName"
                        value={student.lastName}
                        onChange={handleChange}
                        required
                    />
                </Form.Group>
                <Form.Group controlId="dateOfBirth">
                    <Form.Label>Date of Birth</Form.Label>
                    <Form.Control
                        type="date"
                        name="dateOfBirth"
                        value={student.dateOfBirth}
                        onChange={handleChange}
                        required
                    />
                </Form.Group>
                <Button variant="primary" type="submit">
                    {id ? 'Update' : 'Add'}
                </Button>
            </Form>
        </Container>
    );
};

export default AddEditStudent;

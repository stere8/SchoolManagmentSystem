import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate, useParams } from 'react-router-dom';
import { BASE_URL } from '../settings';
import { Form, Button, Container } from 'react-bootstrap';

const AddEditStaff = () => {
    const [staff, setStaff] = useState({ firstName: '', lastName: '', email: '', subjectExpertise: '' });
    const navigate = useNavigate();
    const { id } = useParams();

    useEffect(() => {
        if (id) {
            axios.get(`${BASE_URL}/staff/${id}`)
                .then(response => setStaff(response.data))
                .catch(error => console.error('Error fetching staff data:', error));
        }
    }, [id]);

    const handleChange = e => {
        const { name, value } = e.target;
        setStaff(prevState => ({ ...prevState, [name]: value }));
    };

    const handleSubmit = e => {
        e.preventDefault();
        if (id) {
            axios.put(`${BASE_URL}/staff/${id}`, staff)
                .then(() => navigate('/staff'))
                .catch(error => console.error('Error updating staff:', error));
        } else {
            axios.post(`${BASE_URL}/staff`, staff)
                .then(() => navigate('/staff'))
                .catch(error => console.error('Error adding staff:', error));
        }
    };

    return (
        <Container>
            <h1>{id ? 'Edit Staff' : 'Add Staff'}</h1>
            <Form onSubmit={handleSubmit}>
                <Form.Group controlId="firstName">
                    <Form.Label>First Name</Form.Label>
                    <Form.Control
                        type="text"
                        name="firstName"
                        value={staff.firstName}
                        onChange={handleChange}
                        required
                    />
                </Form.Group>
                <Form.Group controlId="lastName">
                    <Form.Label>Last Name</Form.Label>
                    <Form.Control
                        type="text"
                        name="lastName"
                        value={staff.lastName}
                        onChange={handleChange}
                        required
                    />
                </Form.Group>
                <Form.Group controlId="email">
                    <Form.Label>Email</Form.Label>
                    <Form.Control
                        type="email"
                        name="email"
                        value={staff.email}
                        onChange={handleChange}
                        required
                    />
                </Form.Group>
                <Form.Group controlId="subjectExpertise">
                    <Form.Label>Subject Expertise</Form.Label>
                    <Form.Control
                        type="text"
                        name="subjectExpertise"
                        value={staff.subjectExpertise}
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

export default AddEditStaff;

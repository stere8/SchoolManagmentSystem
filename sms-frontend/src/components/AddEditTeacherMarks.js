import React, {useState, useEffect} from 'react';
import axios from 'axios';
import {useNavigate, useParams} from 'react-router-dom';
import {Form, Button} from 'react-bootstrap';
import {BASE_URL} from '../settings';

const AddEditTeacherMarks = () => {
    const [students, setStudents] = useState([]);
    const [studentId, setStudentId] = useState('');
    const [markValue, setMarkValue] = useState('');
    const {id} = useParams();
    const navigate = useNavigate();

    useEffect(() => {
        const fetchStudents = async () => {
            try {
                const userDetails = JSON.parse(localStorage.getItem('details'));

                if (userDetails && userDetails.assignedClasses.length > 0) {
                    // Fetch students who are in the teacher's assigned classes
                    const classId = userDetails.assignedClasses[0].classId;
                    const response = await axios.get(`${BASE_URL}/students/class/${classId}`);

                    setStudents(response.data);
                    console.log(response.data);
                }
            } catch (error) {
                console.error('There was an error fetching the students!', error);
            }
        };

        // Fetch existing mark details if editing
        const fetchMarkDetails = async () => {
            if (id && id !== '0') {
                try {
                    const response = await axios.get(`${BASE_URL}/marks/${id}`);
                    setStudentId(response.data.studentId);
                    setMarkValue(response.data.markValue);
                } catch (error) {
                    console.error('There was an error fetching the mark details!', error);
                }
            }
        };

        fetchStudents();
        fetchMarkDetails();
    }, [id]);

    const handleSubmit = async (event) => {
        event.preventDefault();
        try {
            const userDetails = JSON.parse(localStorage.getItem('details'));
            if (userDetails && userDetails.assignedClasses.length > 0) {
                const lessonId = userDetails.assignedClasses[0].lessonId;

                const markData = {
                    studentId,
                    lessonId,
                    markValue,
                    date: new Date().toISOString(),
                };

                if (id && id !== '0') {
                    // Update mark if id is provided
                    await axios.put(`${BASE_URL}/marks/${id}`, markData);
                } else {
                    // Create a new mark
                    await axios.post(`${BASE_URL}/marks`, markData);
                }

                navigate('/teachermarks'); // Redirect back to teacher's marks view
            }
        } catch (error) {
            console.error('There was an error saving the mark!', error);
        }
    };

    return (
        <div>
            <h2>{id && id !== '0' ? 'Edit Mark' : 'Add Mark'}</h2>
            <Form onSubmit={handleSubmit}>
                <Form.Group controlId="studentSelect">
                    <Form.Label>Student</Form.Label>
                    <Form.Control
                        as="select"
                        value={studentId}
                        onChange={(e) => setStudentId(e.target.value)}
                        required
                    >
                        <option value="">Select a student</option>
                        {students.map((student) => (
                            <option key={student.studentId} value={student.studentId}>
                                {student.firstName} {student.lastName}
                            </option>
                        ))}
                    </Form.Control>
                </Form.Group>
                <Form.Group controlId="markValue">
                    <Form.Label>Mark</Form.Label>
                    <Form.Control
                        type="number"
                        value={markValue}
                        onChange={(e) => setMarkValue(e.target.value)}
                        placeholder="Enter mark"
                        required
                    />
                </Form.Group>
                <Button variant="primary" type="submit">
                    {id && id !== '0' ? 'Update Mark' : 'Add Mark'}
                </Button>
            </Form>
        </div>
    );
};

export default AddEditTeacherMarks;

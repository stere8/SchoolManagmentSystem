import React from 'react';
import { Navbar, Nav } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';
import authService from '../services/authService';
import { useNavigate } from 'react-router-dom';

const NavigationBar = () => {
    const navigate = useNavigate();
    const user = authService.getCurrentUser();

    const handleLogout = () => {
        authService.logout();
        navigate('/login');
    };

    const handleHome = () => {
        if (user) {
            switch (user.role) {
                case 'parent':
                    navigate('/parent-board');
                    break;
                case 'student':
                    navigate('/student-board');
                    break;
                case 'teacher':
                    navigate('/teacher-board');
                    break;
                default:
                    navigate('/');
            }
        } else {
            navigate('/login');
        }
    };

    return (
        <Navbar bg="dark" variant="dark" expand="lg">
            <Navbar.Brand>School Management System</Navbar.Brand>
            <Navbar.Toggle aria-controls="basic-navbar-nav" />
            <Navbar.Collapse id="basic-navbar-nav">
                <Nav className="mr-auto">
                    <Nav.Link onClick={handleHome}>Home</Nav.Link>
                    {user && user.role === 'admin' && (
                        <>
                            <LinkContainer to="/attendance">
                                <Nav.Link>Attendance</Nav.Link>
                            </LinkContainer>
                            <LinkContainer to="/classes">
                                <Nav.Link>Classes</Nav.Link>
                            </LinkContainer>
                            <LinkContainer to="/enrollments">
                                <Nav.Link>Enrollments</Nav.Link>
                            </LinkContainer>
                            <LinkContainer to="/teacher-enrollments">
                                <Nav.Link>Teacher Enrollments</Nav.Link>
                            </LinkContainer>
                            <LinkContainer to="/lessons">
                                <Nav.Link>Lessons</Nav.Link>
                            </LinkContainer>
                            <LinkContainer to="/marks">
                                <Nav.Link>Marks</Nav.Link>
                            </LinkContainer>
                            <LinkContainer to="/staff">
                                <Nav.Link>Staff</Nav.Link>
                            </LinkContainer>
                            <LinkContainer to="/students">
                                <Nav.Link>Students</Nav.Link>
                            </LinkContainer>
                            <LinkContainer to="/timetable">
                                <Nav.Link>Timetable</Nav.Link>
                            </LinkContainer>
                        </>
                    )}
                    {user && user.role === 'parent' && (
                        <LinkContainer to="/parent-board">
                            <Nav.Link>Parent Board</Nav.Link>
                        </LinkContainer>
                    )}
                    {user && user.role === 'student' && (
                        <LinkContainer to="/student-board">
                            <Nav.Link>Student Board</Nav.Link>
                        </LinkContainer>
                    )}
                    {user && user.role === 'teacher' && (
                        <>
                            <LinkContainer to="/teacher-board">
                                <Nav.Link>Teacher Board</Nav.Link>
                            </LinkContainer>
                            <LinkContainer to="/marks">
                                <Nav.Link>Marks</Nav.Link>
                            </LinkContainer>
                            <LinkContainer to="/attendance">
                                <Nav.Link>Attendance</Nav.Link>
                            </LinkContainer>
                        </>
                    )}
                </Nav>
                <Nav>
                    <Nav.Link onClick={handleLogout}>Logout</Nav.Link>
                </Nav>
            </Navbar.Collapse>
        </Navbar>
    );
};

export default NavigationBar;
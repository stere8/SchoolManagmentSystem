import React from 'react';
import { Navbar, Nav } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';

const NavigationBar = () => {
    return (
        <Navbar bg="dark" variant="dark" expand="lg">
            <Navbar.Brand>School Management System</Navbar.Brand>
            <Navbar.Toggle aria-controls="basic-navbar-nav" />
            <Navbar.Collapse id="basic-navbar-nav">
                <Nav className="mr-auto">
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
                </Nav>
            </Navbar.Collapse>
        </Navbar>
    );
};

export default NavigationBar;

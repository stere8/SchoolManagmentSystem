import React, { useEffect, useState } from 'react';

const ParentDashboard = () => {
    const [userDetails, setUserDetails] = useState(null);

    useEffect(() => {
        const user = JSON.parse(localStorage.getItem('details'));
        setUserDetails(user);
    }, []);

    if (!userDetails) {
        return <p>Loading...</p>;
    }

    return (
        <div>
            <h2>Welcome, {userDetails.FirstName} {userDetails.LastName}</h2>
            <h3>Your Children:</h3>
            <ul>
                {userDetails.Children.map((child) => (
                    <li key={child.StudentId}>
                        {child.FirstName} {child.LastName}, Grade Level: {child.GradeLevel}
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default ParentDashboard;

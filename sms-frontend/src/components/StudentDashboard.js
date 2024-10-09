import React, { useEffect, useState } from 'react';

const StudentDashboard = () => {
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
            <h3>Your Enrolled Classes:</h3>
            <ul>
                {userDetails.EnrolledClasses.map((classItem) => (
                    <li key={classItem.ClassId}>{classItem.ClassName}</li>
                ))}
            </ul>
        </div>
    );
};

export default StudentDashboard;

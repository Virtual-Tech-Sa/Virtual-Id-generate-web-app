import React, { useEffect, useState } from "react";
import { NavLink, Outlet } from "react-router-dom";
import styles from "./NavigatePage.module.css";
import { useNavigate } from 'react-router-dom';

const NavigatePage = () => {
  const navigate = useNavigate();
  const [applicationExists, setApplicationExists] = useState(false);

  useEffect(() => {
  const storedPersonId = localStorage.getItem("personId");
  const personId = storedPersonId ? JSON.parse(storedPersonId) : null;

    if (personId) {
      fetch(`http://localhost:5265/api/Application/CheckExists/${personId}`)
        .then(res => res.json())
        .then(exists => setApplicationExists(exists))
        .catch(() => setApplicationExists(false));
    }
  }, []);


  const handleSignOut = () => {
    // localStorage.removeItem('token');
    // localStorage.removeItem('userId');
    // localStorage.removeItem('currentUserEmail');
    // localStorage.clear(); // Uncomment if you want to clear everything

     localStorage.removeItem('userId');
    localStorage.removeItem('personId');
    navigate('/');
  };

  const handleApplyClick = (e) => {
    if (applicationExists) {
      e.preventDefault();
    }
  };

  return (
    <div className={styles.container}>
      <nav className={styles.sidebar}>
        <h2 className={styles.logoText}>Home Affairs</h2>
        <div className={styles.menu}>
          {applicationExists && (
            <div className={styles.infoMessage}>
              You have already submitted an application.
            </div>
          )}
          {[
            { path: "/apply", label: "Apply" },
            { path: "/dashboard", label: "Applicant Dashboard" },
            { path: "/generate_id", label: "Generate ID" },
          ].map((item) => (
            <div key={item.path} className={styles.menuItem}>
              {item.label === "Apply" ? (
                <NavLink
                  to={item.path}
                  className={({ isActive }) =>
                    isActive ? `${styles.link} ${styles.activeLink}` : styles.link
                  }
                  onClick={handleApplyClick}
                  style={applicationExists ? { pointerEvents: "none", opacity: 0.5, cursor: "not-allowed" } : {}}
                  title={applicationExists ? "You have already submitted an application." : ""}
                >
                  {item.label}
                </NavLink>
              ) : (
                <NavLink
                  to={item.path}
                  className={({ isActive }) =>
                    isActive ? `${styles.link} ${styles.activeLink}` : styles.link
                  }
                >
                  {item.label}
                </NavLink>
              )}

            </div>
          ))}
        </div>
        <div className={styles.signOutContainer}>
          <button className={styles.signOutButton} onClick={handleSignOut}>
            Sign Out
          </button>
        </div>
      </nav>
      <div className={styles.mainContent}>
        <Outlet />
      </div>
    </div>
  );
};

export default NavigatePage;
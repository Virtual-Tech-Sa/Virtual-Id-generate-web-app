import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import styles from './AdminDashboard.module.css';

const TrackingApplications = () => {
  const [applications, setApplications] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [stats, setStats] = useState({
    totalApplications: 0,
    pending: 0,
    approved: 0,
    rejected: 0,
    statusDistribution: []
  });

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await fetch('http://localhost:5265/api/application');
        if (!response.ok) {
          throw new Error('Failed to fetch data');
        }
        const data = await response.json();
        setApplications(data);
        calculateStatistics(data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  const calculateStatistics = (apps) => {
    // Status counts
    const pending = apps.filter(a => a.status?.toLowerCase() === 'pending').length;
    const approved = apps.filter(a => a.status?.toLowerCase() === 'approved').length;
    const rejected = apps.filter(a => a.status?.toLowerCase() === 'rejected').length;
    
    setStats({
      totalApplications: apps.length,
      pending,
      approved,
      rejected,
      statusDistribution: [
        { status: 'Pending', count: pending, color: '#f39c12' },
        { status: 'Approved', count: approved, color: '#2ecc71' },
        { status: 'Rejected', count: rejected, color: '#e74c3c' }
      ]
    });
  };

  if (loading) {
    return (
      <div className={styles.loadingContainer}>
        <div className={styles.spinner}></div>
        <p>Loading tracking data...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className={styles.errorContainer}>
        <div className={styles.errorIcon}>⚠️</div>
        <h2>Error Loading Data</h2>
        <p>{error}</p>
        <button 
          className={styles.retryButton}
          onClick={() => window.location.reload()}
        >
          Retry
        </button>
      </div>
    );
  }

  return (
    <div className={styles.dashboard}>
      {/* Header */}
      <header className={styles.header}>
        <h1>Tracking ID Applications</h1>
        <div className={styles.headerActions}>
        <div className={styles.headerActions}>
      <Link to="/admin-dashboard" className={styles.navButton}>
        <i className="material-icons"></i>Dashboard
      </Link>
    </div>
          <button className={styles.refreshButton} onClick={() => window.location.reload()}>
            <i className="material-icons"></i> Refresh
          </button>
        </div>
      </header>

      {/* Stats Cards */}
      <div className={styles.statsGrid}>
        <div className={`${styles.statCard} ${styles.primary}`}>
          <div className={styles.statIcon}>
            <i className="material-icons">assignment</i>
          </div>
          <div className={styles.statContent}>
            <h3>Total Applications</h3>
            <p className={styles.statValue}>{stats.totalApplications}</p>
            <p className={styles.statTrend}>Last 30 days</p>
          </div>
        </div>

        <div className={`${styles.statCard} ${styles.warning}`}>
          <div className={styles.statIcon}>
            <i className="material-icons">hourglass_empty</i>
          </div>
          <div className={styles.statContent}>
            <h3>Pending</h3>
            <p className={styles.statValue}>{stats.pending}</p>
            <p className={styles.statTrend}>{Math.round((stats.pending / stats.totalApplications) * 100)}% of total</p>
          </div>
        </div>

        <div className={`${styles.statCard} ${styles.success}`}>
          <div className={styles.statIcon}>
            <i className="material-icons">check_circle</i>
          </div>
          <div className={styles.statContent}>
            <h3>Approved</h3>
            <p className={styles.statValue}>{stats.approved}</p>
            <p className={styles.statTrend}>{Math.round((stats.approved / stats.totalApplications) * 100)}% of total</p>
          </div>
        </div>

        <div className={`${styles.statCard} ${styles.danger}`}>
          <div className={styles.statIcon}>
            <i className="material-icons">cancel</i>
          </div>
          <div className={styles.statContent}>
            <h3>Rejected</h3>
            <p className={styles.statValue}>{stats.rejected}</p>
            <p className={styles.statTrend}>{Math.round((stats.rejected / stats.totalApplications) * 100)}% of total</p>
          </div>
        </div>
      </div>

      {/* Charts Section */}
      <div className={styles.chartsSection}>
        <div className={styles.chartCard}>
          <h2>Application Status</h2>
          <div className={styles.pieChart}>
            <div 
              className={styles.pieSegment}
              style={{
                background: `conic-gradient(
                  ${stats.statusDistribution.map((s, i) => 
                    `${s.color} ${i === 0 ? 0 : stats.statusDistribution.slice(0, i).reduce((a, b) => a + (b.count/stats.totalApplications)*100, 0)}% ${stats.statusDistribution.slice(0, i+1).reduce((a, b) => a + (b.count/stats.totalApplications)*100, 0)}%`
                  ).join(', ')}
                )`
              }}
            ></div>
            <div className={styles.pieLegend}>
              {stats.statusDistribution.map(({ status, count, color }) => (
                <div key={status} className={styles.legendItem}>
                  <span className={styles.legendColor} style={{ backgroundColor: color }}></span>
                  <span>{status}: {count}</span>
                </div>
              ))}
            </div>
          </div>
        </div>

        <div className={styles.chartCard}>
          <h2>Recent Activity</h2>
          <div className={styles.activityTimeline}>
            {applications.slice(0, 5).map(app => (
              <div key={app.applicationId} className={styles.timelineItem}>
                <div className={styles.timelineDot}></div>
                <div className={styles.timelineContent}>
                  <div className={styles.timelineHeader}>
                    <span className={styles.appId}>ID: {app.applicationId}</span>
                    <span className={`${styles.statusBadge} ${
                      app.status?.toLowerCase() === 'approved' ? styles.approved :
                      app.status?.toLowerCase() === 'rejected' ? styles.rejected :
                      styles.pending
                    }`}>
                      {app.status || 'Pending'}
                    </span>
                  </div>
                  <div className={styles.timelineDate}>
                    {new Date(app.createdAt || new Date()).toLocaleString()}
                  </div>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>

      {/* Applications Table */}
      <div className={styles.tableSection}>
        <div className={styles.tableHeader}>
          <h2>All Applications</h2>
          <div className={styles.tableControls}>
            <div className={styles.searchBox}>
              <i className="material-icons">search</i>
              <input type="text" placeholder="Search applications..." />
            </div>
            <select className={styles.filterSelect}>
              <option>All Statuses</option>
              <option>Pending</option>
              <option>Approved</option>
              <option>Rejected</option>
            </select>
          </div>
        </div>
        
        <div className={styles.tableContainer}>
          <table className={styles.usersTable}>
            <thead>
              <tr>
                <th>App ID</th>
                <th>Person ID</th>
                <th>Father ID</th>
                <th>Mother ID</th>
                <th>Status</th>
                <th>Created At</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {applications.map((app) => (
                <tr key={app.applicationId}>
                  <td>{app.applicationId}</td>
                  <td>{app.personId}</td>
                  <td>{app.fatherId || 'N/A'}</td>
                  <td>{app.motherId || 'N/A'}</td>
                  <td>
                    <span className={`${styles.statusBadge} ${
                      app.status?.toLowerCase() === 'approved' ? styles.approved :
                      app.status?.toLowerCase() === 'rejected' ? styles.rejected :
                      styles.pending
                    }`}>
                      {app.status || 'Pending'}
                    </span>
                  </td>
                  <td>{new Date(app.createdAt || new Date()).toLocaleDateString()}</td>
                  <td>
                    <button className={styles.actionButton}>
                      <i className="material-icons">visibility</i>
                    </button>
                    <button className={styles.actionButton}>
                      <i className="material-icons">edit</i>
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        <div className={styles.tableFooter}>
          <div className={styles.rowsInfo}>
            Showing 1 to {applications.length} of {applications.length} entries
          </div>
          <div className={styles.pagination}>
            <button disabled className={styles.pageButton}>Previous</button>
            <button className={`${styles.pageButton} ${styles.active}`}>1</button>
            <button className={styles.pageButton}>Next</button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default TrackingApplications;
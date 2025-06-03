import React, { useRef, useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import styles from './CameraPage.module.css';

const CameraPage = () => {
  const videoRef = useRef(null);
  const canvasRef = useRef(null);
  const [capturedImage, setCapturedImage] = useState(null);
  const [cameraStarted, setCameraStarted] = useState(false);
  const [stream, setStream] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    // Cleanup: stop camera when component unmounts
    return () => {
      if (stream) {
        stream.getTracks().forEach(track => track.stop());
      }
    };
  }, [stream]);

  const startCamera = async () => {
    try {
      const mediaStream = await navigator.mediaDevices.getUserMedia({ video: true });
      videoRef.current.srcObject = mediaStream;
      setStream(mediaStream);
      setCameraStarted(true);
    } catch (err) {
      alert('Could not access webcam. Please allow permissions.');
    }
  };

  const captureImage = () => {
    const context = canvasRef.current.getContext('2d');
    context.drawImage(videoRef.current, 0, 0, 640, 480);
    const dataURL = canvasRef.current.toDataURL('image/jpeg');
    setCapturedImage(dataURL);

    // ✅ Stop the camera immediately after capture
    stopCamera();
  };

 const saveImageToServer = async () => {
  try {
    const formData = new FormData();
    const blob = dataURLtoBlob(capturedImage);
    formData.append('photo', blob, 'profile.jpg');

    // Get the response from backend
    const response = await axios.post('http://localhost:5265/api/Person/upload-photo', formData); // this handled in step 3
    const { filePath } = response.data; // <-- This is the correct path

    alert('Photo uploaded successfully!');

    const savedData = JSON.parse(localStorage.getItem('applicationFormData')) || {};
    savedData.ProfilePicture = filePath; // <-- Save the correct path

    localStorage.setItem('applicationFormData', JSON.stringify(savedData));
    // After capturing/uploading and converting to base64:
    localStorage.setItem('profilePictureBase64', capturedImage);
    
    navigate('/apply');
  } catch (error) {
    console.error('Error uploading photo:', error);
    alert('Failed to upload photo.');
  }
};

  function dataURLtoBlob(dataURL) {
    const arr = dataURL.split(',');
    const mime = arr[0].match(/:(.*?);/)[1];
    const bstr = atob(arr[1]);
    let n = bstr.length;
    const u8arr = new Uint8Array(n);
    while (n--) {
      u8arr[n] = bstr.charCodeAt(n);
    }
    return new Blob([u8arr], { type: mime });
  }

  const stopCamera = () => {
    if (stream) {
      stream.getTracks().forEach(track => track.stop());
      setStream(null);
      setCameraStarted(false);
    }
  };

  const retakePhoto = () => {
    setCapturedImage(null);
    startCamera(); // Restart camera
  };

  return (
    <div className={styles.cameraContainer}>
      <h2>Capture Your Profile Picture</h2>

      {/* Only show video if no image has been captured */}
      {!capturedImage && (
        <div className={styles.cameraPreview}>
          <video ref={videoRef} autoPlay playsInline width="640" height="480" />
          <canvas ref={canvasRef} width="640" height="480" style={{ display: 'none' }} />
        </div>
      )}

      {/* Show captured image only after taking photo */}
      {capturedImage && (
        <div className={styles.capturedImage}>
          <img src={capturedImage} alt="Captured" />
        </div>
      )}

      {/* Controls based on state */}
      <div className={styles.cameraControls}>
        {!capturedImage ? (
          <>
            {!cameraStarted ? (
              <button onClick={startCamera}>Start Camera</button>
            ) : (
              <button onClick={captureImage}>Capture</button>
            )}
            {cameraStarted && <button onClick={stopCamera}>Stop Camera</button>}
          </>
        ) : (
          <>
            <button onClick={retakePhoto}>Retake Photo</button>
            <button onClick={saveImageToServer}>Save Photo</button>
          </>
        )}
      </div>
    </div>
  );
};

export default CameraPage;
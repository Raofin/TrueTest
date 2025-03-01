import axios from "axios";

const API_URL = process.env.NEXT_PUBLIC_AUTH_URL

const Axios = axios.create({
  baseURL: API_URL,
  headers: {
    "Content-Type": "application/json",
  },
  withCredentials: true, 
});

Axios.interceptors.response.use(
  (response) => response,
  (error) => {
    console.error("API Error:", error.response?.data?.message || error.message);
    if (error.response?.status === 401) {
      console.warn("Unauthorized! Redirecting to login...");
      window.location.href = "/login"; 
    }
    return Promise.reject(error);
  }
);

export default Axios;

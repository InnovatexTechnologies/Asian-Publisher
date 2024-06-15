import axios from "axios";

export const axiosInstance = axios.create({
  baseURL: `https://api.asianpublisher.in`,
  withCredentials: true,
});
//   baseURL : `https://api.asianpublisher.in/api/BookApi`

export const REACT_APP_URL = "https://api.asianpublisher.in";

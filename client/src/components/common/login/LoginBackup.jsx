import React, { useEffect, useState } from "react";
import AsianLogoText from "../../../Images/AsianLogoText.jpeg";
import login from "../../../Images/Login.png";
import { useNavigate } from "react-router-dom";
import "../../../css/login.css";
import Header from "../header/Header";
import axios from "axios";
import { toastError, toastSuceess } from "../../../util/reactToastify";
import Spinner from "../Spinner";

function Login() {
  const navigate = useNavigate();
  const [loader, setLoader] = useState(false);
  const [empty, setEmpty] = useState(false);
  const [formData, setFormData] = useState({
    email: "",
    password: "",
  });
  const { email, password } = formData;

  function handleChange(event) {
    const { name, value } = event.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  }

  async function handleSubmit(event) {
    event.preventDefault();
    setLoader(true);
    try {
      const response = await axios.get(
        `https://api.asianpublisher.in/api/UserApi?email=${email}&&password=${password}`
      );
      if (response?.data?.message === "Success" && response?.data?.userId) {
        navigate("/order-list", { state: response?.data?.userId });
      } else {
        toastError("You Email or Password is incorrect");
      }
    } catch (error) {
      toastError(error?.response?.data?.message);
    }
    setLoader(false);
  }

  useEffect(() => {
    if (empty) {
      setFormData({
        email: "",
        password: "",
      });
    }
  }, [empty]);

  return (
    <>
      {loader && <Spinner />}

      <link
        href="https://fonts.googleapis.com/css?family=Karla:400,700&display=swap"
        rel="stylesheet"
      />
      <link
        href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css"
        rel="stylesheet"
        integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC"
        crossOrigin="anonymous"
      />
      <link rel="stylesheet" href="login.css" />
      <div style={{ display: "none" }}>
        <Header />
      </div>
      <div
        className="container"
        style={{
          marginTop: 30,
          borderRadius: 20,
          boxShadow: "0 10px 30px 0 rgba(172, 168, 168, 0.43)",
          height: "90vh",
          paddingLeft: 0,
        }}
      >
        <div
          className="col-lg-7"
          style={{
            float: "left",
            backgroundImage: `url(${login})`,
            height: "100%",
            borderTopLeftRadius: 20,
            borderBottomLeftRadius: 20,
            backgroundPosition: "center",
            backgroundSize: "cover",
          }}
        />
        <div
          className="col-lg-5"
          style={{ float: "left", padding: "100px 50px 50px 50px" }}
        >
          <img
            src={AsianLogoText}
            alt="logo"
            style={{ height: "10vh" }}
            className="logo"
          />
          <br />
          <br />
          <form onSubmit={handleSubmit} style={{ maxWidth: "none" }}>
            <div className="form-group">
              <label htmlFor="email" className="sr-only">
                Email
              </label>
              <input
                type="email"
                name="email"
                value={email}
                id="email"
                required
                className="form-control"
                placeholder="Email address"
                onChange={handleChange}
              />
            </div>
            <div className="form-group">
              <label htmlFor="password" className="sr-only">
                Password
              </label>
              <input
                type="password"
                name="password"
                value={password}
                id="password"
                required
                className="form-control"
                placeholder="Password"
                onChange={handleChange}
              />
            </div>
            <center>
              <input
                name="login"
                id="login"
                className="login-btn mb-4"
                type="submit"
                value="SIgn In Now"
                style={{
                  backgroundColor: "#D82028",
                  color: "#fff",
                  border: "none",
                  padding: "10px 20px 10px 20px",
                }}
              />
            </center>
          </form>
        </div>
      </div>
    </>
  );
}

export default Login;

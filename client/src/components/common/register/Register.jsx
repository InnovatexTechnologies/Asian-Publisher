import React from "react";
import AsianLogoText from "../../../Images/AsianLogoText.jpeg";
import login from "../../../Images/Login.png";
import "../../../css/login.css";
import Header from "../header/Header";

function Register() {
  return (
    <>
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
          style={{ float: "left", padding: "50px 50px 50px 50px" }}
        >
          <img
            src={AsianLogoText}
            alt="logo"
            style={{ height: "10vh" }}
            className="logo"
          />
          <br />
          <br />
          <form action="#!" style={{ maxWidth: "none" }}>
            <div className="form-group">
              <label htmlFor="Name" className="sr-only">
                Name
              </label>
              <input
                type="text"
                name="Name"
                id="Name"
                className="form-control"
                placeholder="Name"
              />
            </div>
            <div className="form-group">
              <label htmlFor="email" className="sr-only">
                Email
              </label>
              <input
                type="email"
                name="email"
                id="email"
                className="form-control"
                placeholder="Email address"
              />
            </div>
            <div className="form-group">
              <label htmlFor="Mobile" className="sr-only">
                Mobile No.
              </label>
              <input
                type="text"
                name="Mobile"
                id="Mobile"
                className="form-control"
                placeholder="Mobile No."
              />
            </div>
            <div className="form-group">
              <label htmlFor="password" className="sr-only">
                Password
              </label>
              <input
                type="password"
                name="password"
                id="password"
                className="form-control"
                placeholder="Password"
              />
            </div>
            <center>
              <input
                name="login"
                id="login"
                className="login-btn mb-4"
                type="button"
                defaultValue="Sign Up Now"
                style={{
                  backgroundColor: "#D82028",
                  color: "#fff",
                  border: "none",
                  padding: "10px 20px 10px 20px",
                }}
              />
            </center>
          </form>
          <p className="login-card-footer-text">
            Don't have an account?
            <a href="/Login" className="text-reset">
              Login here
            </a>
          </p>
        </div>
      </div>
    </>
  );
}

export default Register;

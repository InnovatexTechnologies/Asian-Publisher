import React, { useEffect, useState } from "react";
import Header from "../../common/header/Header";
import Footer from "../../common/footer/Footer";
import { createBecomeAuthor } from "../../../redux/slices/becomeAuthorSlice";
import { useDispatch, useSelector } from "react-redux";
import becomeanauthor from "../../../Images/becomeanauthor.png";
import { Button } from "@mui/material";
function BecomeanAuthor() {
  const { loading, becomeAuthors } = useSelector((state) => state.becomeAuthor);
  const dispatch = useDispatch();
  const [formData, setFormData] = useState({
    name: "",
    email: "",
    mobileNo: "",
    address: "",
    message: "",
  });
  const { name, email, mobileNo, address, message } = formData;

  /**handle change method here  */
  function handleChange(event) {
    const { name, value } = event.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  }

  /**handle submit method here  */
  function handleSubmit(event) {
    event.preventDefault();

    let becomeAuthorsData = new FormData();
    becomeAuthorsData.append("name", name);
    becomeAuthorsData.append("email", email);
    becomeAuthorsData.append("mobileNo", mobileNo);
    becomeAuthorsData.append("address", address);
    becomeAuthorsData.append("message", message);

    /** hitt the create become author api from  here */
    dispatch(createBecomeAuthor(becomeAuthorsData));
  }
  useEffect(() => {
    if (loading === "fulfilled") {
      setFormData({
        name: "",
        email: "",
        mobileNo: "",
        address: "",
        message: "",
      });
    }
  }, [loading]);

  return (
    <>
      <Header />
      <div
        className="Headerrowabout"
        style={{
          backgroundImage: `url(${becomeanauthor})`,
          backgroundSize: "cover",
          backgroundPosition: "center",
        }}
      >
        <div className="gradient-overlay" />
        <div className="col-lg-2" style={{ float: "left" }}>
          &nbsp;
        </div>
        <div className="col-lg-8" style={{ float: "left" }}>
          <div
            className="video-content"
            style={{ textAlign: "center", paddingTop: 150 }}
          >
            <p
              className="text"
              style={{
                textTransform: "uppercase",
                fontSize: 50,
                fontWeight: 600,
                textShadow: "2px 2px 4px rgba(0, 0, 0, 0.5)",
              }}
            >
              Become an Author
            </p>
            <p style={{ fontSize: 20 }}>
              Asian Publishers is your life long Learning Partner. We have
              empowered the growth of Students,Teachers, &amp; Professionals
              since 1981.
            </p>
            {}
          </div>
        </div>
        <div className="col-lg-2" style={{ float: "left" }}>
          &nbsp;
        </div>
      </div>
      <br />
      <div
        className="row"
        id="AboutUsSection"
        style={{ display: "block", margin: 0, padding: 0, clear: "both" }}
      >
        <form onSubmit={handleSubmit}>
          <div className="container">
            <div className="col-lg-6" style={{ float: "left" }}>
              <label style={{ fontWeight: 600 }}>Name</label>
              <input
                className="form-controlCustomized"
                type="text"
                placeholder="Name"
                name="name"
                value={formData.name}
                required
                onChange={handleChange}
                // defaultValue=""
              />
            </div>
            <div className="col-lg-6" style={{ float: "left" }}>
              <label style={{ fontWeight: 600 }}>Email</label>
              <input
                className="form-controlCustomized"
                type="text"
                defaultValue=""
                placeholder="Email"
                name="email"
                value={formData.email}
                required
                onChange={handleChange}
                // defaultValue=""
              />
            </div>
            <div className="col-lg-6" style={{ float: "left", marginTop: 20 }}>
              <label style={{ fontWeight: 600 }}>Mobile No.</label>
              <input
                className="form-controlCustomized"
                type="text"
                placeholder="Mobile No."
                name="mobileNo"
                value={formData.mobileNo}
                required
                onChange={handleChange}
                // defaultValue=""
              />
            </div>
            <div className="col-lg-6" style={{ float: "left", marginTop: 20 }}>
              <label style={{ fontWeight: 600 }}>Address</label>
              <input
                className="form-controlCustomized"
                type="text"
                placeholder="Address"
                name="address"
                value={formData.address}
                required
                onChange={handleChange}
                // defaultValue=""
              />
            </div>
            <div className="col-lg-12" style={{ float: "left", marginTop: 20 }}>
              <label style={{ fontWeight: 600 }}>Message</label>
              <textarea
                rows={5}
                className="form-control"
                placeholder="Message"
                name="message"
                value={formData.message}
                required
                onChange={handleChange}
              />
            </div>
            <div className="col-lg-12" style={{ float: "left", marginTop: 40 }}>
              <center>
                <Button
                  // href="#"
                  type="submit"
                  style={{
                    textDecoration: "none",
                    backgroundColor: "#d82028",
                    color: "#fff",
                    padding: "10px 50px 10px 50px",
                    marginTop: 20,
                    borderRadius: 10,
                  }}
                >
                  Submit Now
                </Button>
              </center>
            </div>
          </div>
        </form>
      </div>
      <Footer />
    </>
  );
}

export default BecomeanAuthor;

import React, { useEffect, useState } from "react";
import Header from "../../common/header/Header";
import Footer from "../../common/footer/Footer";
import useContactUs from "../../../hooks/useContactUs";
import Spinner from "../../common/Spinner";
import contactusheaderimage from "../../../Images/contactusheaderimage.png";
import AsianLogo from "../../../Images/AsianLogo.jpeg";
import AsianLogoText from "../../../Images/AsianLogoText.jpeg";

function ContactUs() {
  const { loading, submitData } = useContactUs();
  const [formData, setFormData] = useState({
    name: "",
    email: "",
    mobileNo: "",
    address: "",
    message: "",
  });
  function handleChange(event) {
    const { name, value } = event.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  }
  function handleSubmit(event) {
    event.preventDefault();
    submitData(formData);
  }
  useEffect(() => {
    if (!loading) {
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
      {/* {loading && <Spinner />}
      <title>Contact Us - Asian Publishers</title> */}
      <>
        <Header />
        <div
          className="Headerrowabout"
          style={{
            backgroundImage: `url(${contactusheaderimage})`,
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
                Contact Us
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
          <div className="container-fluid">
            <div
              className="col-lg-4"
              style={{
                height: "70vh",
                float: "left",
                background:
                  "linear-gradient(to top, rgb(216, 32, 40, 0.1), #fff)",
                padding: "0px 50px 0px 50px",
                borderBottomLeftRadius: 500,
                borderBottomRightRadius: 500,
              }}
            >
              <h4
                style={{
                  textAlign: "center",
                  fontSize: 25,
                  textTransform: "uppercase",
                  fontWeight: 600,
                  textShadow: "2px 2px 4px rgba(0, 0, 0, 0.5)",
                }}
              >
                Contact Info
              </h4>
              <br />
              <br />
              <div className="row">
                <div className="col-1">
                  <i style={{ fontSize: 18 }} className="fas fa-map-marker" />
                </div>
                <div className="col-10 footercontact">
                  <p style={{ fontSize: 16 }}>
                    46/20, Kambal Wala Bagh, Jansath Road, Opp. Punjab National
                    Bank, New Mandi, Muzaffarnagar - 251001
                  </p>
                </div>
              </div>
              <div className="row">
                <div className="col-1">
                  <i style={{ fontSize: 18 }} className="fas fa-phone-square" />
                </div>
                <div className="col-10 footercontact">
                  <p style={{ fontSize: 16 }}>
                    +91 9412639492 <br />
                    +91 98736 20572
                  </p>
                </div>
              </div>
              <div className="row">
                <div className="col-1">
                  <i style={{ fontSize: 18 }} className="fas fa-envelope" />
                </div>
                <div className="col-10 footercontact">
                  <p style={{ fontSize: 16 }}>
                    spmittal@asianpublishers.co.in, sales@asianpublishers.co.in
                  </p>
                </div>
              </div>
            </div>
            <div className="col-lg-8" style={{ float: "left" }}>
              <h4
                style={{
                  textAlign: "center",
                  fontSize: 30,
                  textTransform: "uppercase",
                  fontWeight: 600,
                  textShadow: "2px 2px 4px rgba(0, 0, 0, 0.5)",
                }}
              >
                Feel Free to Contact Now
              </h4>
              <br />
              <div className="col-lg-6" style={{ float: "left" }}>
                <label style={{ fontWeight: 600 }}>Name</label>
                <input
                  className="form-controlCustomized"
                  type="text"
                  placeholder="Name"
                  name="name"
                  required
                  value={formData.name}
                  onChange={handleChange}
                  //defaultValue=""
                />
              </div>
              <div className="col-lg-6" style={{ float: "left" }}>
                <label style={{ fontWeight: 600 }}>Email</label>
                <input
                  className="form-controlCustomized"
                  type="text"
                  placeholder="Email"
                  name="email"
                  required
                  value={formData.email}
                  onChange={handleChange}
                  // defaultValue=""
                />
              </div>
              <div
                className="col-lg-6"
                style={{ float: "left", marginTop: 20 }}
              >
                <label style={{ fontWeight: 600 }}>Mobile No.</label>
                <input
                  className="form-controlCustomized"
                  type="text"
                  placeholder="Mobile No."
                  name="mobileNo"
                  required
                  value={formData.mobileNo}
                  onChange={handleChange}
                  //defaultValue=""
                />
              </div>
              <div
                className="col-lg-6"
                style={{ float: "left", marginTop: 20 }}
              >
                <label style={{ fontWeight: 600 }}>Address</label>
                <input
                  className="form-controlCustomized"
                  type="text"
                  placeholder="Address"
                  name="address"
                  required
                  value={formData.address}
                  onChange={handleChange}
                  //defaultValue=""
                />
              </div>
              <div
                className="col-lg-12"
                style={{ float: "left", marginTop: 20 }}
              >
                <label style={{ fontWeight: 600 }}>Message</label>
                <textarea
                  rows={5}
                  className="form-control"
                  placeholder="Your Message"
                  name="message"
                  required
                  value={formData.message}
                  onChange={handleChange}
                />
              </div>
              <div
                className="col-lg-12"
                style={{ float: "left", marginTop: 40 }}
              >
                <center>
                  <a
                    href="#"
                    onClick={handleSubmit}
                    style={{
                      textDecoration: "none",
                      backgroundColor: "#d82028",
                      color: "#fff",
                      padding: "10px 50px 10px 50px",
                      marginTop: 20,
                      borderRadius: 10,
                    }}
                  >
                    CONTACT NOW
                  </a>
                </center>
              </div>
            </div>
          </div>
        </div>
        <div className="row" style={{ clear: "both", height: "5vh" }} />
        <div className="row" style={{ clear: "both" }}>
          <iframe
            src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d55574.58198434516!2d77.66765778469876!3d29.475194078566478!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x390c1ba00983523d%3A0xc6a75ba008574871!2sMuzaffarnagar%2C%20Uttar%20Pradesh!5e0!3m2!1sen!2sin!4v1713521929069!5m2!1sen!2sin"
            width="100%"
            height={400}
            style={{ border: 0 }}
            allowFullScreen=""
            loading="lazy"
            referrerPolicy="no-referrer-when-downgrade"
          />
        </div>
        <Footer />
      </>
    </>
  );
}

export default ContactUs;

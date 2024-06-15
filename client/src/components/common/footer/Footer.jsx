import React from "react";
import AsianLogo from "../../../Images/AsianLogoText.jpeg";

function Footer() {
  return (
    <>
      <div className="row" style={{ clear: "both", height: "2vh" }} />
      <div
        className="row"
        style={{
          backgroundColor: "white",
          position: "relative",
          padding: "50px 30px 0px 30px",
          textAlign: "left",
          color: "black",
        }}
      >
        {/* <div class="row"> */}
        <div className="col-md-4 col-12" style={{ padding: 10 }}>
          <img src={AsianLogo} style={{ height: "10vh", borderRadius: 10 }} />
          <br />
          <br />
          <p style={{ textAlign: "justify" }}>
            Asian Publishers was established in 1981. Since then, it has grown
            to be the leading academic publisher of low-cost and high-quality
            Diploma Education in Uttar Pradesh. With the help and support of our
            experienced authors, Asian Publishers has worked to empower the
            growth of students, teachers and professionals.
            <a
              style={{ textDecoration: "none", color: "#d82028" }}
              href="/about-us"
            >
              read more...
            </a>
          </p>
        </div>
        <div className="col-md-4 col-12" style={{ padding: 10 }}>
          <h2 style={{ textAlign: "left", fontSize: 25 }}>Connect With Us</h2>
          <hr />
          <div style={{ marginTop: 5, marginBottom: 5 }}>
            <div className="row">
              <div className="col-1">
                <i style={{ fontSize: 18 }} className="fas fa-map-marker" />
              </div>
              <div className="col-10 footercontact">
                <p>
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
                <p>
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
                <p>
                  spmittal@asianpublishers.co.in, sales@asianpublishers.co.in
                </p>
              </div>
            </div>
            <span id="siteseal"></span>
          </div>
        </div>
        <div className="col-md-4 col-12" style={{ padding: 10 }}>
          <h2 style={{ textAlign: "left", fontSize: 25 }}>Location</h2>
          <hr />
          <iframe
            src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d55574.58198434516!2d77.66765778469876!3d29.475194078566478!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x390c1ba00983523d%3A0xc6a75ba008574871!2sMuzaffarnagar%2C%20Uttar%20Pradesh!5e0!3m2!1sen!2sin!4v1713521929069!5m2!1sen!2sin"
            width="100%"
            height={200}
            style={{ border: 0 }}
            allowFullScreen=""
            loading="lazy"
            referrerPolicy="no-referrer-when-downgrade"
          />
           <a href="/PrivacyPolicy" style={{textDecoration:"none", color:"#000"}}>Privacy Policy</a> &nbsp;&nbsp;
          <a href="/TermsandCondition" style={{textDecoration:"none", color:"#000"}}>Terms & Condition</a> &nbsp;&nbsp;
          <a href="/CancelationPolicy" style={{textDecoration:"none", color:"#000"}}>Cancelation Policy</a>
       
        </div>
        {/* </div> */}
        <div
          className="row"
          style={{
            boxShadow: "0px -5px 5px rgba(0, 0, 0, 0.2)",
            paddingTop: 10,
            width: "100vw",
          }}
        >
          <center>
            <p style={{ textAlign: "center", width: "100vw" }}>
              Designed &amp; Developed By Innovate X
            </p>
          </center>
        </div>
      </div>
    </>
  );
}

export default Footer;

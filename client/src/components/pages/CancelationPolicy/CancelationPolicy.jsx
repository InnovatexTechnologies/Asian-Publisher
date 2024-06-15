import React from "react";
import Header from "../../common/header/Header";
import Footer from "../../common/footer/Footer";
import CartHeaderImage from "../../../Images/CartHeaderImage.png";

const CancelationPolicy = () => {
  return (
    <>
      <Header />
      <div
        className="Headerrowabout"
        style={{
          backgroundImage: `url(${CartHeaderImage})`,
          "background-size": "cover",
          "background-position": "center",
        }}
      >
        <div className="gradient-overlay" />
        <div className="col-lg-2" style={{ float: "left" }}>
          &nbsp;
        </div>
        <div className="col-lg-8" style={{ float: "left" }}>
          <div
            className="video-content"
            style={{
              "-webkit-text-align": "center",
              "text-align": "center",
              "padding-top": "150px",
            }}
          >
            <p
              className="text"
              style={{
                "-webkit-text-transform": "uppercase",
                "text-transform": "uppercase",
                "font-size": "50px",
                "font-weight": "600",
                "-webkit-text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.5)",
                "text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.5)",
              }}
            >
              Cancelation Policy
            </p>
            <p style={{ "font-size": "20px" }}>
              Asian Publishers is your life long Learning Partner. We have
              empowered the growth of Students,Teachers, &amp; Professionals
              since 1981.
            </p>
          </div>
        </div>
        <div className="col-lg-2" style={{ float: "left" }}>
          &nbsp;
        </div>
      </div>
      <br />
      <div class="page">
  <div class="header">
    <div class="container">
      <p class="title">Cancellation Only Policy for Asian Publishers</p>
    </div>
  </div>
  <div class="translations-content-container">
    <div class="container">
      <div class="tab-content translations-content-item en visible" id="en">
        <h1>Cancellation Policy</h1>
        <p>Last updated: May 11, 2024</p>
        <p>Thank you for shopping at Asian Publishers.</p>
        <p>
          If, for any reason, You are not completely satisfied with a purchase,
          We invite You to review our Cancellation Policy.
        </p>
        <p>
          The following terms are applicable for any products that You purchased
          with Us.
        </p>
        <h2>Your Order Cancellation Rights</h2>
        <p>
          Within 4 hours of placing Your Order, You have the right to cancel it
          and provide a valid reason.
        </p>
        <ul>
          <li>
            <p>
              To cancel your order within the 4 hours, please log in to your
              account panel on our website.
            </p>
          </li>
        </ul>
        <h2>Conditions for Order Cancellation</h2>
        <p>
          In order for the Order to be eligible for cancellation, please make
          sure that:
        </p>
        <ul>
          <li>The Order was placed within the last 4 hours</li>
        </ul>
        <p>
          We reserve the right to refuse cancellations of any orders that do not
          meet the above conditions in our sole discretion.
        </p>
        <h3>Contact Us</h3>
        <p>
          If you have any questions about our Cancellation Policy, please
          contact us:
        </p>
        <ul>
          <li>
            <p>
              By email: <a>spmittal@asianpublishers.co.in</a>,
              <a>sales@asianpublishers.co.in</a>
            </p>
          </li>
          <li>
            <p>By phone number: +91 9412639492, +91 9873620572</p>
          </li>
        </ul>
      </div>
    </div>
  </div>
</div>

      <Footer />
    </>
  );
};

export default CancelationPolicy;

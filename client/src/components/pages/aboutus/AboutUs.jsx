import React from "react";
import Header from "../../common/header/Header";
import Footer from "../../common/footer/Footer";
// import "./Aboutus.css";
import AsianLogo from "../../../Images/AsianLogo.jpeg";
import AboutHeaderImage from "../../../Images/AboutHeaderImage.png";
import AboutUsImage from "../../../Images/AboutUs2.jpeg";
import WhoWeAreImage from "../../../Images/WhoWeAreImage.png";
import WhyUsImage from "../../../Images/WhyUsImage.png";
import Director from "../../../Images/Director.jpg";
import AsianLogoText from "../../../Images/AsianLogoText.jpeg";
import AboutUsSlider from "../aboutus/AboutUsSlider";
import AboutUsImgArr from "../aboutus/aboutUsImg";

function AboutUs() {
  return (
    <>
      <Header />
      <div
        className="Headerrowabout"
        style={{
          backgroundImage: `url(${AboutHeaderImage})`,
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
              About Us
            </p>
            <p style={{ fontSize: 20 }}>
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
      <div
        className="row"
        id="AboutUsSection"
        style={{ display: "block", margin: 0, padding: 0, clear: "both" }}
      >
        <p
          style={{
            textAlign: "center",
            fontSize: 15,
            textTransform: "uppercase",
            textShadow: "2px 2px 4px rgba(0, 0, 0, 0.2)",
          }}
        >
          Our Story
          <br />
          <span
            style={{
              textAlign: "center",
              fontSize: 40,
              textTransform: "uppercase",
              fontWeight: 600,
              textShadow: "2px 2px 4px rgba(0, 0, 0, 0.5)",
            }}
          >
            Know About <span style={{ color: "red" }}>Asian Publisher</span>
          </span>
        </p>
        <br />
        <div className="col-lg-6" style={{ float: "left" }}>
          <img src={AboutUsImage} className="Knowmorestyle" />
          {/* <AboutUsSlider slider={AboutUsImgArr} /> */}
        </div>
        <div className="col-lg-6" style={{ float: "left" }}>
          <div className="col-lg-1" style={{ float: "left" }} />
          <div className="col-lg-11 aboutcontent">
            <p
              style={{
                fontSize: 22,
                fontWeight: 600,
                textShadow: "2px 2px 10px rgba(0, 0, 0, 0.5)",
              }}
            >
              " Helping Teachers to Teach and Students to Learn "
            </p>
            <p style={{ fontSize: 18 }}>
              Asian Publishers was established in 1981. Since then, it has grown
              to be the leading academic publisher of low-cost and high-quality
              Diploma Education in Uttar Pradesh. With the help and support of
              our experienced authors, Asian Publishers has worked to empower
              the growth of students, teachers and professionals.
              <br />
              <br />
              The name “Asian Publishers, Muzaffarnagar” has been echoed all
              over Uttar Pradesh and Uttarakhand for almost three generations in
              the field of Engineering and Polytechnic books.
              <br />
              <br />
              And as we are constantly strengthening our core foundations by
              introducing new and updated versions of our books every year, we
              are overwhelmed by the support shown by students and teachers
              throughout Uttar Pradesh which motivates us, even more, to
              influence as many students as we can by publishing top-grade study
              material.
            </p>
          </div>
        </div>
      </div>
      <div className="row" style={{ clear: "both", height: "5vh" }} />
      <div
        className="row"
        style={{
          clear: "both",
          background:
            "linear-gradient(to bottom, rgb(238, 238, 238, 0.4), #fff)",
          padding: 40,
        }}
      >
        <div className="container">
          <div className="col-lg-7" style={{ float: "left" }}>
            <p
              style={{
                textAlign: "left",
                fontSize: 15,
                textTransform: "uppercase",
                textShadow: "2px 2px 4px rgba(0, 0, 0, 0.2)",
              }}
            >
              <span
                style={{
                  textAlign: "left",
                  fontSize: 40,
                  textTransform: "uppercase",
                  fontWeight: 600,
                  textShadow: "2px 2px 4px rgba(0, 0, 0, 0.5)",
                }}
              >
                Who <span style={{ color: "red" }}>We are ?</span>
              </span>
            </p>
            <p style={{ textAlign: "justify", width: "80%" }}>
              Asian Publishers, Muzaffarnagar is one of the largest and leading
              academic publishers in Uttar Pradesh.Low-cost and high-quality
              affordable texts of Asian Publishers, Muzaffarnagar have been the
              pioneer in learning states of UP and UK. We have, so far, release
              more than 250 titles encompassing various branches: Civil
              Engineering, Electrical Engineering, Mechanical Engineering,
              Electronics Engineering, Computer Science Engineering and
              Information Technology and so on.
            </p>
          </div>
          <div
            className="col-lg-5"
            style={{
              float: "left",
              backgroundImage: `url(${WhoWeAreImage})`,
              height: "50vh",
              borderTopLeftRadius: 100,
              borderBottomRightRadius: 100,
              backgroundPosition: "center",
              backgroundSize: "cover",
            }}
          />
        </div>
      </div>
      <div className="row" style={{ clear: "both", height: "2vh" }} />
      <div
        className="row"
        style={{
          clear: "both",
          background: "linear-gradient(to top, rgb(216, 32, 40, 0.1), #fff)",
          padding: 40,
        }}
      >
        <div className="container">
          <div
            className="col-lg-5"
            style={{
              float: "left",
              backgroundImage: `url(${WhyUsImage})`,
              height: "70vh",
              borderTopLeftRadius: 100,
              borderBottomRightRadius: 100,
              backgroundPosition: "center",
              backgroundSize: "cover",
            }}
          />
          <div className="col-lg-7" style={{ float: "left" }}>
            <div className="col-lg-2" style={{ float: "left" }}>
              &nbsp;
            </div>
            <div className="col-lg-10" style={{ float: "left" }}>
              <p
                style={{
                  textAlign: "right",
                  fontSize: 15,
                  textTransform: "uppercase",
                  textShadow: "2px 2px 4px rgba(0, 0, 0, 0.2)",
                }}
              >
                <span
                  style={{
                    textAlign: "right",
                    fontSize: 40,
                    textTransform: "uppercase",
                    fontWeight: 600,
                    textShadow: "2px 2px 4px rgba(0, 0, 0, 0.5)",
                  }}
                >
                  Why <span style={{ color: "red" }}>Us ?</span>
                </span>
              </p>
              <p style={{ textAlign: "justify" }}>
                Our books are designed in a way to effectively cover the
                syllabus prescribed by the Board of Technical Education, Uttar
                Pradesh &amp; Uttarakhand. Our authors strive to provide the
                virtue of simple and sound learning through various effective
                methods in their books. We not only try to cover the syllabus
                prescribed by the Board of Technical Education but also go the
                extra mile to make sure that the students fully understand not
                just the theoretical but the practical aspects of the respective
                subject as well.
                <br />
                <br />
                You should choose Asian Publishers because we promise to
                continuously upgrade our standards to provide the best learning
                material and set the highest benchmarks for ourselves and
                everyone in the industry.
              </p>
            </div>
          </div>
        </div>
      </div>
      <div
        className="row"
        id="number-scroller"
        style={{
          clear: "both",
          backgroundColor: "#d82028",
          padding: "25px 10px 25px 10px",
        }}
      >
        <div className="col-lg-4" style={{ float: "left", padding: 25 }}>
          <div className="col-lg-7 numstyle1">
            <div className="number" data-count={35}>
              0
            </div>
          </div>
          <div className="col-lg-5 numstyle2">Years of Experience</div>
        </div>
        <div className="col-lg-4" style={{ float: "left", padding: 25 }}>
          <div className="col-lg-7 numstyle1">
            <div className="number" data-count={80}>
              0
            </div>
          </div>
          <div className="col-lg-5 numstyle2">Experienced Authors</div>
        </div>
        <div className="col-lg-4" style={{ float: "left", padding: 25 }}>
          <div className="col-lg-7 numstyle1">
            <div className="number" data-count={300}>
              0
            </div>
          </div>
          <div className="col-lg-5 numstyle2">Books Titles</div>
        </div>
      </div>
      <div
        className="row"
        style={{
          clear: "both",
          background: "linear-gradient(to top, rgb(216, 32, 40, 0.1), #fff)",
          padding: 40,
        }}
      >
        <div className="container-fluid">
          <div className="col-lg-7" style={{ float: "left" }}>
            <p
              style={{
                textAlign: "left",
                fontSize: 15,
                textTransform: "uppercase",
                textShadow: "2px 2px 4px rgba(0, 0, 0, 0.2)",
              }}
            >
              <span
                style={{
                  textAlign: "left",
                  fontSize: 40,
                  textTransform: "uppercase",
                  fontWeight: 600,
                  textShadow: "2px 2px 4px rgba(0, 0, 0, 0.5)",
                }}
              >
                THE JOURNEY TO
                <span style={{ color: "red" }}> IMPACT LIVES…</span>
              </span>
            </p>
            <p style={{ textAlign: "justify", width: "90%" }}>
              Our founder, Shri Satya Prakash Mittal started his journey to
              impact the lives of millions of students with only a single title
              under his name.
              <br />
              <br />
              With consistent efforts to fully achieve his goal of spreading
              education and knowledge through simplistic learning methods, Asian
              publishers now have over 250 titles under its name. Sh. Satya
              Prakash Mittal has given Asian Publishers its true values of
              making our nation great through work on the grassroots level via
              education and learning.
              <br />
              <br />
              Through his efforts, Asian Publishers is one of the biggest names
              in the higher education industry in the Indian states of
              Uttarakhand and Uttar Pradesh!
            </p>
          </div>
          <div
            className="col-lg-5"
            style={{
              float: "left",
              backgroundImage: `url(${Director})`,
              height: "70vh",
              borderTopLeftRadius: "100px",
              borderBottomRightRadius: "100px",
              backgroundPosition: "center",
              backgroundSize: "cover",
            }}
          ></div>
        </div>
      </div>
      <Footer />
    </>
  );
}

export default AboutUs;

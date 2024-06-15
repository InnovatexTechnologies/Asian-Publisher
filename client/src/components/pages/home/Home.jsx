import React, { useState, useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { fetchBooks } from "../../../redux/slices/bookSlice";
import { fetchCourses } from "../../../redux/slices/courseSlice";
import BookSlider from "../../common/slider/BookSlider";
import CourseSlider from "../../common/slider/CourseSlider";
import Header from "../../common/header/Header";
import Footer from "../../common/footer/Footer";
import AboutUs from "../../../Images/AboutUs3.jpeg";
import HomeVideo from "../../../Images/HomeVideo.mp4";
import courseData from "../../common/slider/courseData";
// import "../../../css/Style.css";
import "../../../css/bootstrap.min.css";
import Spinner from "../../common/Spinner";

function Home() {
  const imgMainArr = [
    "../../../Images/Logo/1.jpg",
    "../../../Images/Logo/11.png",
    "../../../Images/Logo/2.jpg",
    "../../../Images/Logo/22.png",
    "../../../Images/Logo/4.jpg",
    "../../../Images/Logo/44.png",
    "../../../Images/Logo/5.jpg",
    "../../../Images/Logo/55.png",
    "../../../Images/Logo/6.jpg",
    "../../../Images/Logo/66.png",
    "../../../Images/Logo/3.jpg",
  ];
  const { loading, books } = useSelector((state) => state.book);
  const { loading: coursesLoading, courses } = useSelector(
    (state) => state.course
  );
  const dispatch = useDispatch();
  const [allBooks, setAllBooks] = useState([]);
  const [loader, setLoader] = useState(true);
  const [allCourses, setAllCourses] = useState([]);
  useEffect(() => {
    if (allBooks?.length > 0) {
      setLoader(false);
    }
  }, [allCourses, allBooks]);
  useEffect(() => {
    dispatch(fetchBooks({}));
  }, [dispatch]);
  useEffect(() => {
    dispatch(fetchCourses());
  }, [dispatch]);
  useEffect(() => {
    if (loading === "fulfilled") {
      setAllBooks(books);
    }
  }, [loading]);
  useEffect(() => {
    if (coursesLoading === "fulfilled") {
      setAllCourses(courses);
    }
  }, [coursesLoading]);
  return (
    <>
      {loader && <Spinner />}
            <Header />
      <div className="Headerrow">
        <div className="video-background">
          <video autoPlay loop muted className="video homeheadervideo">
            <source src={HomeVideo} type="video/mp4" />
          </video>
        </div>
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
              "padding-top": "200px",
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
              Asian Publishers
            </p>
            <p style={{ "font-size": "20px" }}>
              Asian Publishers is your life long Learning Partner. We have
              empowered the growth of Students,Teachers, &amp; Professionals
              since 1981.
            </p>
            <br />
            <a
              href="/shop"
              style={{
                color: "#fff",
                "background-color": "#d82028",
                padding: "10px 30px 10px 30px",
                "-webkit-text-decoration": "none",
                "text-decoration": "none",
                "border-radius": "5px",
                "box-shadow": "4px 4px 6px rgba(0, 0, 0, 0.3)",
              }}
            >
              SHOP NOW
            </a>
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
        style={{
          display: "block",
          margin: "0px",
          padding: "0px",
          clear: "both",
        }}
      >
        <p
          style={{
            "-webkit-text-align": "center",
            "text-align": "center",
            "font-size": "15px",
            "-webkit-text-transform": "uppercase",
            "text-transform": "uppercase",
            "-webkit-text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.2)",
            "text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.2)",
          }}
        >
          Our Story
          <br />
          <span
            style={{
              "-webkit-text-align": "center",
              "text-align": "center",
              "font-size": "40px",
              "-webkit-text-transform": "uppercase",
              "text-transform": "uppercase",
              "font-weight": "600",
              "-webkit-text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.5)",
              "text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.5)",
            }}
          >
            Know About <span style={{ color: "red" }}>Asian Publisher</span>
          </span>
        </p>
        <br />
        {/* <div className="col-lg-6" style={{ float: "left" }}>
          <img src={AboutUs} className="Knowmorestyle" />
        </div> */}
        <div className="col-lg-6" style={{ float: "left" }}>
          {/* <AboutUsSlider slider={AboutUsImgArr} /> */}
          <img src={AboutUs} className="Knowmorestyle" />
        </div>
        <div className="col-lg-6" style={{ float: "left" }}>
          <div className="col-lg-1" style={{ float: "left" }} />
          <div className="col-lg-11 aboutcontent">
            <p
              style={{
                "font-size": "22px",
                "font-weight": "600",
                "-webkit-text-shadow": "2px 2px 10px rgba(0, 0, 0, 0.5)",
                "text-shadow": "2px 2px 10px rgba(0, 0, 0, 0.5)",
              }}
            >
              " Helping Teachers to Teach and Students to Learn "
            </p>
            <p style={{ "font-size": "18px" }}>
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
        id="number-scroller"
        style={{
          clear: "both",
          "background-color": "#d82028",
          padding: "25px 10px 25px 10px",
        }}
      >
        <div className="col-lg-4" style={{ float: "left", padding: "25px" }}>
          <div className="col-lg-7 numstyle1">
            <div className="number" data-count={35}>
              0
            </div>
          </div>
          <div className="col-lg-5 numstyle2">Years of Experience</div>
        </div>
        <div className="col-lg-4" style={{ float: "left", padding: "25px" }}>
          <div className="col-lg-7 numstyle1">
            <div className="number" data-count={80}>
              0
            </div>
          </div>
          <div className="col-lg-5 numstyle2">Experienced Authors</div>
        </div>
        <div className="col-lg-4" style={{ float: "left", padding: "25px" }}>
          <div className="col-lg-7 numstyle1">
            <div className="number" data-count={300}>
              0
            </div>
          </div>
          <div className="col-lg-5 numstyle2">Books Titles</div>
        </div>
      </div>
      <div className="row" style={{ clear: "both", height: "5vh" }} />
      <div className="row featuredbookdiv">
        <p
          style={{
            "-webkit-text-align": "center",
            "text-align": "center",
            "font-size": "15px",
            "-webkit-text-transform": "uppercase",
            "text-transform": "uppercase",
            "-webkit-text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.2)",
            "text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.2)",
          }}
        >
          Our Books
          <br />
          <span
            style={{
              "-webkit-text-align": "center",
              "text-align": "center",
              "font-size": "40px",
              "-webkit-text-transform": "uppercase",
              "text-transform": "uppercase",
              "font-weight": "600",
              "-webkit-text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.5)",
              "text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.5)",
            }}
          >
            Top Selling <span style={{ color: "red" }}>Books</span>
          </span>
        </p>
        <br />
        <div className="container-fluid">
          {allBooks && allBooks.length > 0 && <BookSlider slider={allBooks} />}
        </div>
      </div>
      <div
        className="row"
        style={{
          display: "block",
          margin: "0px",
          padding: "0px",
          clear: "both",
        }}
      >
        <br />
        <br />
        <p
          style={{
            "-webkit-text-align": "center",
            "text-align": "center",
            "font-size": "15px",
            "-webkit-text-transform": "uppercase",
            "text-transform": "uppercase",
            "-webkit-text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.2)",
            "text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.2)",
          }}
        >
          Asian Books
          <br />
          <span
            style={{
              "-webkit-text-align": "center",
              "text-align": "center",
              "font-size": "40px",
              "-webkit-text-transform": "uppercase",
              "text-transform": "uppercase",
              "font-weight": "600",
              "-webkit-text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.5)",
              "text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.5)",
            }}
          >
            Books <span style={{ color: "red" }}>Available for</span>
          </span>
        </p>
        <br />
        <div className="col-lg-12" style={{ float: "left" }}>
          {allCourses && allCourses.length > 0 && (
            <CourseSlider slider={courseData} />
          )}
        </div>
      </div>
      <div
        className="row"
        style={{
          display: "block",
          margin: "0px",
          padding: "0px 0px 20px 0px",
          clear: "both",
          background: "linear-gradient(to bottom, rgb(216, 32, 40, 0.1), #fff)",
        }}
      >
        <br />
        <br />
        <p
          style={{
            "-webkit-text-align": "center",
            "text-align": "center",
            "font-size": "15px",
            "-webkit-text-transform": "uppercase",
            "text-transform": "uppercase",
            "-webkit-text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.2)",
            "text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.2)",
          }}
        >
          Our Books
          <br />
          <span
            style={{
              "-webkit-text-align": "center",
              "text-align": "center",
              "font-size": "40px",
              "-webkit-text-transform": "uppercase",
              "text-transform": "uppercase",
              "font-weight": "600",
              "-webkit-text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.5)",
              "text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.5)",
            }}
          >
            How <span style={{ color: "red" }}>Our Books</span> Will Help You
          </span>
        </p>
        <br />
        <div className="col-lg-12 bookhelpstyle">
          <p>
            We guide our students throughout the path to unlock their highest
            potential. We understand the value of education and hence offer the
            finest content developed by highly qualified and experienced authors
            who spare no effort to make the books exceptional.
          </p>
          <br />
          <p>
            <b>• AICTE recognized - </b>The majority of our books have been
            awarded by the AICTE, Government of India.
          </p>
          <p>
            <b>• Language-Friendly - </b>We provide books in English as well as
            in Hindi for each subject such that English and Hindi medium
            students can study together in one class.
          </p>
          <p>
            <b>• Theoretical concepts explained vividly -</b>Our books offer
            theoretical concepts explained in an elaborate yet easy to
            understand manner with the help of diagrams and illustrations so
            that the students can have sound knowledge that goes beyond the
            reach of mainstream books in the market.
          </p>
          <p>
            <b>• Brimming with a wide range of practice questions - </b>Each
            chapter includes practice questions ranging from low to high level
            difficulty.
          </p>
          <p>
            <b>• Provide effective summaries - </b>The authors have included
            comprehensive summaries at the end of each chapter so that students
            can quickly review their syllabus.
          </p>
          <p>
            <b>• Affordable and easily available - </b>We make our books widely
            and easily available at affordable market prices so that everybody
            can benefit from them.
          </p>
          <p>
            <b>• Extensive syllabus coverage - </b>Our books are designed to
            cover the entire new syllabus for UP and UK Board of Technical
            Education, so that our students do not need to hunt for other
            resources.
          </p>
          <p>
            <b>• Design - </b>Our books are presented in such a beautiful and an
            organized way that the students can never get bored and rather enjoy
            studying the subject.
          </p>
        </div>
      </div>
      <div className="row" style={{ clear: "both", height: "5vh" }} />
      <p
        style={{
          "-webkit-text-align": "center",
          "text-align": "center",
          "font-size": "15px",
          "-webkit-text-transform": "uppercase",
          "text-transform": "uppercase",
          "-webkit-text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.2)",
          "text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.2)",
        }}
      >
        Asian Publishers
        <br />
        <span
          style={{
            "-webkit-text-align": "center",
            "text-align": "center",
            "font-size": "40px",
            "-webkit-text-transform": "uppercase",
            "text-transform": "uppercase",
            "font-weight": "600",
            "-webkit-text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.5)",
            "text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.5)",
          }}
        >
          Extreme Colleges <span style={{ color: "red" }}>We Are Serving</span>
        </span>
      </p>
      <div class="slider">
        <div class="slide-track">
          {imgMainArr?.map((item, index) => (
            <div key={index} class="slide">
              <img
                src={item}
                style={{
                  height: "100%",
                  width: "150px",
                }}
              />
            </div>
          ))}
        </div>
      </div>

      <Footer />
    </>
  );
}

export default Home;

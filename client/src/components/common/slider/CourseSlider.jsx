import React from "react";
import Slider from "react-slick";
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";
const CourseSlider = ({ slider, type = "" }) => {
  const settings = {
    dots: false,
    infinite: true,
    speed: 4000,
    autoplay: true,
    autoplaySpeed: 2000,
    slidesToShow: 3,
    slidesToScroll: 1,
  };

  // {
  //   console.log("slider", slider);
  // }
  return (
    <div className="col-lg-12">
      <Slider {...settings}>
        {slider.map((item, index) => {
          return <CarousalItem key={index} {...item} type={type} />;
        })}
      </Slider>
    </div>
  );
};

const CarousalItem = ({ image, name, desc }) => {
  const strArr = name.split(" ");
  const remainsName = strArr.slice(-1);
  const firstName = strArr.slice(0, -1).join(" ");

  return (
    <>
      <div className="col-lg-12" style={{ float: "left" }}>
        <img src={image} className="homecoursesstyle" />
        <p
          style={{
            "font-size": "20px",
            "-webkit-text-align": "center",
            "text-align": "center",
            "margin-top": "15px",
            "font-weight": "600",
            "-webkit-text-transform": "uppercase",
            "text-transform": "uppercase",
            "-webkit-text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.2)",
            "text-shadow": "2px 2px 4px rgba(0, 0, 0, 0.2)",
          }}
        >
          {firstName} <span style={{ color: "red" }}>{remainsName}</span>
          <br />
          <span
            style={{
              "font-size": "15px",
              "font-weight": "normal",
              "-webkit-text-transform": "none",
              "text-transform": "none",
              color: "#000",
              "-webkit-text-shadow": "none",
              "text-shadow": "none",
            }}
          >
            {desc}
          </span>
        </p>
      </div>
    </>
  );
};

export default CourseSlider;

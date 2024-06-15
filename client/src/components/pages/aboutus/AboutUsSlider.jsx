import React from "react";
import Slider from "react-slick";
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";
import AboutUs from "../../../Images/AboutUs.png";
import "../../../css/Style.css";
import "../../../css/bootstrap.min.css";

const AboutUsSlider = ({ slider, type = "" }) => {
  const settings = {
    dots: false,
    infinite: true,
    speed: 3000,
    autoplay: false,
    autoplaySpeed: 1000,
    slidesToShow: 3,
    slidesToScroll: 1,
  };

  return (
    <Slider {...settings}>
      {slider.map((item, index) => {
        return <CarousalItem key={index} image={item} type={type} />;
      })}
    </Slider>
  );
};

const CarousalItem = ({ image }) => {
  return (
    <>
      <img src={image} className="Knowmorestyle" />
    </>
  );
};

export default AboutUsSlider;

import React from "react";
import Slider from "react-slick";
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";
import "./LogoSlider.css";

const LogoSlider = ({ slider, type = "" }) => {
  const settings = {
    dots: false,
    infinite: true,
    speed: 5000,
    autoplay: true,
    autoplaySpeed: 5000,
    slidesToShow: 5,
    slidesToScroll: 1,
  };

  return (
    <div className="row">
      <div className="col-sm-12">
        <div id="customers-testimonials" className="owl-carousel">
          {slider.length === 1 ? (
            <CarousalItem image={slider[0]} type={type} />
          ) : (
            <Slider {...settings}>
              {slider.map((item, index) => (
                <CarousalItem key={index} image={item} type={type} />
              ))}
            </Slider>
          )}
        </div>
      </div>
    </div>
  );
};

const CarousalItem = ({ image, type }) => {
  return (
    <div className="brand-logo-slider-wrapper brand-logo-slider-wrapper--double-border">
      <div
        className="brand_slider_active"
        data-slick='{
                           "slidesToShow": 5,
                           "arrows": false,
                           "slidesToScroll": 1,
                           "responsive":[
                           {"breakpoint":1800, "settings": {"slidesToShow": 5 }},
                           {"breakpoint":1200, "settings": {"slidesToShow": 4 }},        
                           {"breakpoint":991, "settings": {"slidesToShow": 3 }},
                           {"breakpoint":767, "settings": {"slidesToShow": 2 }},        
                           {"breakpoint":319, "settings": {"slidesToShow": 1 }}
                           ] 
                           }'
      >
        <div className="single-brand-logo" style={{ width: "1%" }}>
          <a href="#">
            <img src={image} alt="" style={{ height: "10vh" }} />
          </a>
        </div>
      </div>
    </div>
  );
};

export default LogoSlider;

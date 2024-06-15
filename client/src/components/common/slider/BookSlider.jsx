import React from "react";
import Slider from "react-slick";
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";
import { REACT_APP_URL } from "../../../config/config";
const BookSlider = ({ slider, type = "" }) => {
  const settings = {
    dots: false,
    infinite: true,
    speed: 3000,
    autoplay: true,
    autoplaySpeed: 2000,
    slidesToShow: 4,
    slidesToScroll: 1,
  };

  return (
    <div className="row">
      <div className="col-sm-12">
        <Slider {...settings}>
          {slider.map((item, index) => {
            if (item.isFeatured === true) {
              return <CarousalItem key={index} {...item} type={type} />;
            }
          })}
        </Slider>
      </div>
    </div>
  );
};

const CarousalItem = ({ image, name, iSBN, authors, mRP, type }) => {
  return (
    <>
      <div className="col-lg-12" style={{ float: "left" }}>
        <center>
          <div className="book">
            <img
              className="book-cover"
              src={`${REACT_APP_URL}/Image/${image}`}
              alt="Book Cover"
            />
            <div className="effect"></div>
            <div className="light"></div>
          </div>
        </center>

        <p
          style={{
            "font-size": "15px",
            "-webkit-text-align": "center",
            "text-align": "center",
            "margin-top": "15px",
            "font-weight": "700",
          }}
        >
          <span style={{ "font-size": "12px", "font-weight": "500" }}>
            ISBN No. {iSBN}
          </span>
          <br />
          {name}
          <br />
          {authors && authors.length > 0 && (
            <a href="#" className="remove_href">
              {authors.map((author, index) => (
                <span
                  key={author.id}
                  style={{ "font-size": "12px", "font-weight": "500" }}
                >
                  {author.name}
                  {index !== authors.length - 1 ? ", " : ""}
                </span>
              ))}
            </a>
          )}
          <br />
          <span
            style={{
              color: "red",
              "font-size": "16px",
              "font-weight": "600",
            }}
          >
            Rs. {mRP}
          </span>
        </p>
      </div>
    </>
  );
};

export default BookSlider;

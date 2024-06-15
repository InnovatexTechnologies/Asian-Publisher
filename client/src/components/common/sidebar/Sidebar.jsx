import React from "react";
import "./Sidebar.css";
import { FaWhatsapp } from "react-icons/fa";

const Sidebar = () => {
  return (
    <div className="sticky-container" id="Floating">
      <ul className="sticky FloatingulClass">
        <li className="Hoverclass" title="whatsapp">
          <a href="https://web.whatsapp.com/send?phone=919873620572" target="_blank" rel="noopener noreferrer">
            <FaWhatsapp style={{ fontSize: "28px", paddingLeft: "7px", color:"#fff" }} />
          </a>
        </li>
      </ul>
    </div>
  );
};

export default Sidebar;

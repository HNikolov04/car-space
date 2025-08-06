import { useState, useEffect, useRef } from "react";
import { Link, useLocation } from "react-router-dom";
import { ROUTES } from "../../utils/routes/routes";

import CarSpaceLogo from "../../assets/CarSpaceWhiteModeLogo.png";
import DefaultUserPFP from "../../assets/CarSpaceUserDefaultWhiteModePfp.png";
import ProfilePictureDropDownMenu from "../shared/ProfilePictureDropDownMenu";
import SearchBar from "../shared/SearchBar";
import { useAuth } from "../../contexts/AuthContext";

export default function CarMeetHeader({ searchTerm, setSearchTerm }) {
  const { isAuthenticated, user } = useAuth();
  const [dropdownOpen, setDropdownOpen] = useState(false);
  const [openMenu, setOpenMenu] = useState(null);
  const dropdownRef = useRef(null);
  const location = useLocation();
  const currentPath = location.pathname;

  useEffect(() => {
    const handleClickOutside = (e) => {
      if (dropdownRef.current && !dropdownRef.current.contains(e.target)) {
        setDropdownOpen(false);
      }
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  const baseClass =
    "text-base font-semibold px-4 py-2 rounded-md shadow transition duration-200 border border-gray-300";
  const activeClass = "bg-blue-600 text-white";
  const inactiveClass = "text-black bg-white hover:bg-blue-600 hover:text-white";

  const navLinks = [
    { label: "Home", to: ROUTES.home },
    { label: "Meets", to: ROUTES.carMeet },
    { label: "Forum", to: ROUTES.carForum },
    { label: "Services", to: ROUTES.carService },
    { label: "Cars for Sale", to: ROUTES.carShop },
    { label: "About", to: ROUTES.about },
    { label: "Contact Us", to: ROUTES.contact },
  ];

  return (
    <header className="bg-white border-b border-gray-200 px-6 h-[80px] flex items-center justify-between relative z-50 shadow">
      {/* Logo + Search */}
      <div className="flex items-center gap-6">
        <div className="relative w-[150px] h-[60px] flex items-center justify-center select-none">
          <Link to={ROUTES.home} className="absolute inset-0 z-10" />
          <img
            src={CarSpaceLogo}
            alt="CarSpace Logo"
            className="h-12 w-auto scale-[4] -translate-y-1 pointer-events-none"
          />
        </div>

        <div className="w-[360px]">
          <SearchBar
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            placeholder="Search meets..."
          />
        </div>
      </div>

      {/* Center Nav */}
      <nav className="absolute left-1/2 top-1/2 -translate-x-1/2 -translate-y-1/2 flex gap-4">
        {navLinks.map(({ label, to }) => (
          <Link
            key={label}
            to={to}
            className={`${baseClass} ${currentPath === to ? activeClass : inactiveClass}`}
          >
            {label}
          </Link>
        ))}
      </nav>

      {/* Right: Management + Profile */}
      <div className="relative flex items-center gap-3" ref={dropdownRef}>
        {/* Admin-only Management button */}
        {isAuthenticated && user?.role === "Administrator" && (
          <Link
            to={ROUTES.management}
            className={`${baseClass} ${currentPath === ROUTES.management ? activeClass : inactiveClass
              }`}
          >
            Management
          </Link>
        )}

        {isAuthenticated ? (
          <div className="relative">
            <div
              onClick={() => setDropdownOpen((prev) => !prev)}
              className="flex items-center gap-2 cursor-pointer select-none px-3 py-1 hover:bg-gray-100 rounded-full"
            >
              <img
                src={
                  user?.profileImageUrl
                    ? `https://localhost:7192${user.profileImageUrl}`
                    : DefaultUserPFP
                }
                onError={(e) => (e.target.src = DefaultUserPFP)}
                alt="User avatar"
                className="w-12 h-12 rounded-full border border-gray-400 object-cover"
              />
              <span className="text-sm font-medium text-black">{user?.username}</span>
              <span className="text-sm">▼</span>
            </div>

            {dropdownOpen && (
              <div className="absolute right-0 mt-2 z-50" onClick={(e) => e.stopPropagation()}>
                <ProfilePictureDropDownMenu
                  openMenu={openMenu}
                  setOpenMenu={setOpenMenu}
                />
              </div>
            )}
          </div>
        ) : (
          <>
            <Link
              to={ROUTES.login}
              className="text-base font-semibold text-black bg-white hover:bg-blue-600 hover:text-white px-4 py-2 rounded-md shadow border border-gray-300 transition"
            >
              Sign In
            </Link>
            <Link
              to={ROUTES.register}
              className="text-base font-semibold text-white bg-blue-600 hover:bg-blue-700 px-4 py-2 rounded-md shadow border border-gray-300 transition"
            >
              Sign Up
            </Link>
          </>
        )}
      </div>
    </header>
  );
}

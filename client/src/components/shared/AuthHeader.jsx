import { useState, useEffect } from "react";
import { Link, useLocation } from "react-router-dom";
import CarSpaceLogo from "../../assets/CarSpaceWhiteModeLogo.png";
import DefaultUserPFP from "../../assets/CarSpaceUserDefaultWhiteModePfp.png";
import ProfilePictureDropDownMenu from "./ProfilePictureDropDownMenu";
import { ROUTES } from "../../utils/routes/routes";
import { useAuth } from "../../contexts/AuthContext";

export default function AuthHeader() {
  const [showDropdown, setShowDropdown] = useState(false);
  const [openMenu, setOpenMenu] = useState(null);
  const { user } = useAuth();
  const isAuthenticated = !!user;

  const location = useLocation();
  const currentPath = location.pathname;

  const baseClass =
    "text-base font-semibold px-4 py-2 rounded-md shadow transition duration-200 border border-gray-300";
  const activeClass = "bg-blue-600 text-white";
  const inactiveClass = "text-black bg-white hover:bg-blue-600 hover:text-white";

  useEffect(() => {
    const handleClickOutside = () => setShowDropdown(false);
    if (showDropdown) {
      window.addEventListener("click", handleClickOutside);
    }
    return () => {
      window.removeEventListener("click", handleClickOutside);
    };
  }, [showDropdown]);

  return (
    <header className="relative bg-white border-b border-gray-200 w-full h-[80px] flex items-center px-6 shadow">
      <div className="relative w-[150px] h-[60px] flex items-center justify-center select-none">
        <Link to={ROUTES.home} className="absolute inset-0 z-10" />
        <img
          src={CarSpaceLogo}
          alt="CarSpace Logo"
          className="h-12 w-auto scale-[4] -translate-y-1 pointer-events-none"
        />
      </div>

      <nav className="absolute left-1/2 top-1/2 -translate-x-1/2 -translate-y-1/2 flex gap-4">
        {[
          { label: "Home", to: ROUTES.home },
          { label: "Meets", to: ROUTES.carMeet },
          { label: "Forum", to: ROUTES.carForum },
          { label: "Services", to: ROUTES.carService },
          { label: "Cars for Sale", to: ROUTES.carShop },
          { label: "About", to: ROUTES.about },
          { label: "Contact Us", to: ROUTES.contact },
        ].map(({ label, to }) => (
          <Link
            key={label}
            to={to}
            className={`${baseClass} ${
              currentPath === to ? activeClass : inactiveClass
            }`}
          >
            {label}
          </Link>
        ))}
      </nav>

      <div className="ml-auto flex-shrink-0 flex items-center gap-4 z-10 relative">
        {isAuthenticated ? (
          <div
            className="relative flex items-center gap-2 cursor-pointer select-none"
            onClick={(e) => {
              e.stopPropagation();
              setShowDropdown((prev) => !prev);
            }}
          >
            <img
              src={
                user?.profileImageUrl
                  ? `https://localhost:7192${user.profileImageUrl}`
                  : DefaultUserPFP
              }
              onError={(e) => (e.target.src = DefaultUserPFP)}
              alt="User avatar"
              className="w-12 h-12 rounded-full border border-gray-400 object-cover cursor-pointer"
            />
            {showDropdown && (
              <div onClick={(e) => e.stopPropagation()}>
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
              className="text-base font-semibold text-black bg-white hover:bg-blue-600 hover:text-white px-4 py-2 rounded-md shadow transition border border-gray-300"
            >
              Sign In
            </Link>
            <Link
              to={ROUTES.register}
              className="text-base font-semibold text-white bg-blue-600 hover:bg-blue-700 px-4 py-2 rounded-md shadow transition border border-gray-300"
            >
              Sign Up
            </Link>
          </>
        )}
      </div>
    </header>
  );
}

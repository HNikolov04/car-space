import { Link, useNavigate } from "react-router-dom";
import { ROUTES } from "../../utils/routes/routes";
import { useAuth } from "../../contexts/AuthContext";
import DefaultUserPFP from "../../assets/CarSpaceUserDefaultWhiteModePfp.png";

export default function ProfilePictureDropDownMenu({ openMenu, setOpenMenu }) {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const toggle = (name) => {
    setOpenMenu((prev) => (prev === name ? null : name));
  };

  const navigateTo = (route, type, mode) => {
    const query = getQueryFor(type, mode);
    navigate(query ? `${route}?${query}` : route);
    setOpenMenu(null);
  };

  const getQueryFor = (type, mode) => {
    switch (type) {
      case "carService":
        return mode === "mine"
          ? "MyServicesOnly=true"
          : mode === "saved"
          ? "SavedOnly=true"
          : "";
      case "carForum":
        return mode === "mine"
          ? "MyArticlesOnly=true"
          : mode === "saved"
          ? "SavedOnly=true"
          : "";
      case "carShop":
        return mode === "mine"
          ? "MyListingsOnly=true"
          : mode === "saved"
          ? "SavedOnly=true"
          : "";
      case "carMeet":
        return mode === "mine"
          ? "MyMeetsOnly=true"
          : mode === "joined"
          ? "JoinedOnly=true"
          : mode === "saved"
          ? "SavedOnly=true"
          : "";
      default:
        return "";
    }
  };

  const linkClass =
    "block text-sm font-medium text-black hover:bg-blue-600 hover:text-white px-4 py-2 rounded transition duration-200 cursor-pointer";

  const sectionWrapper = "relative";
  const caret = "ml-2 text-sm";
  const submenuWrapper =
    "absolute right-full top-0 mr-2 bg-white border border-gray-200 rounded-lg shadow-lg z-50 p-2 space-y-1 min-w-[180px]";

  const renderDropdown = (label, name, routes) => (
    <div className={sectionWrapper}>
      <div
        className={`${linkClass} flex justify-between items-center`}
        onClick={() => toggle(name)}
      >
        {label}
        <span className={caret}>▼</span>
      </div>
      {openMenu === name && (
        <div className={submenuWrapper}>
          {routes.map(({ label, mode }) => (
            <div
              key={label}
              onClick={() => navigateTo(ROUTES[name], name, mode)}
              className={linkClass}
            >
              {label}
            </div>
          ))}
        </div>
      )}
    </div>
  );

  return (
    <div className="absolute right-0 top-full mt-2 w-64 bg-white border border-gray-300 rounded-xl shadow-xl z-50 p-3 space-y-2">
      <Link
        to={ROUTES.profile}
        className="flex items-center gap-3 px-2 pb-3 border-b border-gray-200  rounded transition"
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
        <div className="flex flex-col">
          <span className="text-sm font-semibold text-black">{user?.username}</span>
          <span className="text-xs text-gray-600 truncate max-w-[180px]">
            {user?.email}
          </span>
        </div>
      </Link>

      <Link to={ROUTES.profile} className={linkClass}>
        Profile
      </Link>

      {renderDropdown("CarService", "carService", [
        { label: "My Services", mode: "mine" },
        { label: "Saved Services", mode: "saved" },
      ])}

      {renderDropdown("CarForum", "carForum", [
        { label: "My Articles", mode: "mine" },
        { label: "Saved Articles", mode: "saved" },
      ])}

      {renderDropdown("CarShop", "carShop", [
        { label: "My Car Offers", mode: "mine" },
        { label: "Saved Car Offers", mode: "saved" },
      ])}

      {renderDropdown("CarMeet", "carMeet", [
        { label: "My Meets", mode: "mine" },
        { label: "Joined Meets", mode: "joined" },
        { label: "Saved Meets", mode: "saved" },
      ])}

      <Link to={ROUTES.settings} className={linkClass}>
        Settings
      </Link>

      <div
        onClick={logout}
        className="block text-sm font-medium text-black hover:bg-red-600 hover:text-white px-4 py-2 rounded transition duration-200 cursor-pointer"
      >
        Logout
      </div>
    </div>
  );
}

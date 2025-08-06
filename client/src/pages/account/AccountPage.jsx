import { useLocation, useNavigate } from "react-router-dom";
import AboutHeader from "../../components/shared/AboutHeader";
import Footer from "../../components/shared/Footer";
import { ROUTES } from "../../utils/routes/routes";
import { useEffect, useState } from "react";
import { useAuth } from "../../contexts/AuthContext";
import { deleteUser } from "../../services/api/userService";
import DefaultUserPFP from "../../assets/CarSpaceUserDefaultWhiteModePfp.png";
import axios from "axios";

export default function AccountPage() {
  const location = useLocation();
  const navigate = useNavigate();
  const { user, logout } = useAuth();

  const [activeTab, setActiveTab] = useState("profile");
  const [nickname, setNickname] = useState("");
  const [currentPassword, setCurrentPassword] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [profilePic, setProfilePic] = useState(null);

  useEffect(() => {
    if (!user) {
      navigate("/", { replace: true });
    }
  }, [user, navigate]);

  useEffect(() => {
    if (location.pathname.includes("settings")) {
      setActiveTab("settings");
    } else {
      setActiveTab("profile");
    }
  }, [location.pathname]);

  useEffect(() => {
    if (user) {
      setNickname(user.username || "");
    }
  }, [user]);

  const handleFileChange = (e) => {
    setProfilePic(e.target.files[0]);
  };

  const handleSave = async () => {
    try {
      const formData = new FormData();
      formData.append("NewUsername", nickname);
      formData.append("CurrentPassword", currentPassword);
      formData.append("NewPassword", newPassword);
      formData.append("ConfirmPassword", confirmPassword);
      if (profilePic) {
        formData.append("ProfileImage", profilePic);
      }

      await axios.put("https://localhost:7192/User/update", formData, {
        headers: {
          "Content-Type": "multipart/form-data",
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
        withCredentials: true,
      });

      alert("Profile updated successfully!");
      window.location.reload();
    } catch (err) {
      alert(
        "Failed to update profile: " +
          (err.response?.data || err.message || "Unknown error")
      );
    }
  };

  const handleDeleteAccount = async () => {
    const confirmed = window.confirm("Are you sure you want to delete your account?");
    if (!confirmed) return;

    try {
      await deleteUser();
      logout();
      navigate("/");
    } catch (err) {
      alert("Failed to delete account: " + err.message);
    }
  };

  const sidebarLinkClass = (tab) =>
    `px-4 py-2 rounded text-left w-full font-semibold transition ${
      activeTab === tab ? "bg-blue-600 text-white" : "text-black hover:bg-gray-200 cursor-pointer"
    }`;

  return (
    <>
      <AboutHeader />
      <main className="bg-gray-50 min-h-screen flex flex-col items-center py-12 px-4">
        <div className="bg-white w-full max-w-4xl rounded-2xl shadow-xl flex overflow-hidden border border-gray-200 mt-6 mb-12">
          {/* Sidebar */}
          <div className="w-1/3 border-r border-gray-300 p-6 bg-white flex flex-col items-center">
            {/* Profile Picture Upload + Info */}
            <div className="flex flex-col items-center mb-6">
              <div className="relative group mb-2">
                <img
                  src={
                    user?.profileImageUrl
                      ? `https://localhost:7192/${user.profileImageUrl}`
                      : DefaultUserPFP
                  }
                  alt="Profile"
                  className="w-24 h-24 rounded-full object-cover border border-gray-300"
                />
                <label className="absolute inset-0 bg-black bg-opacity-40 rounded-full flex items-center justify-center opacity-0 group-hover:opacity-100 transition cursor-pointer">
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    className="h-6 w-6 text-white"
                    fill="none"
                    viewBox="0 0 24 24"
                    stroke="currentColor"
                  >
                    <path
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      strokeWidth={2}
                      d="M15.232 5.232l3.536 3.536M9 13h3l9-9a1.414 1.414 0 00-2-2l-9 9v3z"
                    />
                  </svg>
                  <input type="file" onChange={handleFileChange} className="hidden" />
                </label>
              </div>

              <p className="text-lg font-semibold text-gray-900">{user?.username}</p>

              <p className="text-sm text-gray-600">{user?.email}</p>
            </div>

            <div className="flex flex-col gap-2 w-full">
              <button
                onClick={() => navigate(ROUTES.profile)}
                className={sidebarLinkClass("profile")}
              >
                Profile
              </button>
              <button
                onClick={() => navigate(ROUTES.settings)}
                className={sidebarLinkClass("settings")}
              >
                Settings
              </button>
              <button
                onClick={logout}
                className="px-4 py-2 rounded text-left w-full text-red-500 font-semibold hover:bg-gray-200 cursor-pointer"
              >
                Logout
              </button>
              <button
                onClick={handleDeleteAccount}
                className="px-4 py-2 rounded text-left w-full text-red-600 font-semibold hover:bg-gray-100 cursor-pointer"
              >
                Delete Account
              </button>
            </div>
          </div>

          <div className="flex-1 p-6">
            {activeTab === "profile" && (
              <>
                <h2 className="text-2xl font-bold mb-6 text-gray-900">Edit Profile</h2>
                <div className="space-y-6">
                  <div>
                    <label className="italic text-base text-gray-700">Nickname</label>
                    <input
                      type="text"
                      value={nickname}
                      onChange={(e) => setNickname(e.target.value)}
                      className="w-full px-4 py-2 border border-gray-300 rounded-lg mt-1 text-base"
                    />
                  </div>
                  <div>
                    <label className="italic text-base text-gray-700">Current password</label>
                    <input
                      type="password"
                      value={currentPassword}
                      onChange={(e) => setCurrentPassword(e.target.value)}
                      className="w-full px-4 py-2 border border-gray-300 rounded-lg mt-1 text-base"
                    />
                  </div>
                  <div>
                    <label className="italic text-base text-gray-700">New password</label>
                    <input
                      type="password"
                      value={newPassword}
                      onChange={(e) => setNewPassword(e.target.value)}
                      className="w-full px-4 py-2 border border-gray-300 rounded-lg mt-1 text-base"
                    />
                  </div>
                  <div>
                    <label className="italic text-base text-gray-700">Confirm new password</label>
                    <input
                      type="password"
                      value={confirmPassword}
                      onChange={(e) => setConfirmPassword(e.target.value)}
                      className="w-full px-4 py-2 border border-gray-300 rounded-lg mt-1 text-base"
                    />
                  </div>

                  <button
                    onClick={handleSave}
                    className="bg-blue-600 hover:bg-blue-700 text-white px-6 py-2 rounded-xl font-bold transition cursor-pointer"
                  >
                    Save Changes
                  </button>
                </div>
              </>
            )}

            {activeTab === "settings" && (
              <>
                <h2 className="text-2xl font-bold mb-6 text-gray-900">Settings</h2>
                <div className="space-y-6">
                  <div>
                    <label className="italic text-base text-gray-700 block mb-2">Language</label>
                    <select
                      className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-300 text-base"
                      defaultValue="english"
                    >
                      <option value="english">English</option>
                      <option value="bulgarian">Bulgarian</option>
                    </select>
                  </div>
                  <div>
                    <label className="italic text-base text-gray-700 block mb-2">Theme</label>
                    <select
                      className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-300 text-base"
                      defaultValue="light"
                    >
                      <option value="light">Light</option>
                      <option value="dark">Dark</option>
                    </select>
                  </div>
                </div>
              </>
            )}
          </div>
        </div>
      </main>
      <Footer />
    </>
  );
}

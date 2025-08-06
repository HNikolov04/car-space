import { useNavigate } from "react-router-dom";
import MainPageHeader from "../../components/shared/MainPageHeader";
import Footer from "../../components/shared/Footer";
import { useAuth } from "../../contexts/AuthContext";
import { useEffect } from "react";

export default function ManagementPage() {
  const navigate = useNavigate();

  const { user } = useAuth();

  useEffect(() => {
    if (!user || user.role !== "Administrator") {
      navigate("/");
    }
  }, [user, navigate]);

  const sections = [
    { title: "Manage About Us", route: "/management/about/edit", comingSoon: false },
    { title: "Manage Contact Us", route: "/management/contact/edit", comingSoon: false },
    { title: "Manage Car Service Categories", route: "/management/service-categories", comingSoon: false },
    { title: "Manage Car Forum Brands", route: "/management/forum-brands", comingSoon: false },
    { title: "Manage Car Shop Brands", route: "/management/shop-brands", comingSoon: false },
    { title: "Manage Users", comingSoon: true },
    { title: "Manage Car Meets", comingSoon: true },
    { title: "Manage Car Services", comingSoon: true },
    { title: "Manage Car Forums", comingSoon: true },
    { title: "Manage Car Shop", comingSoon: true },
  ];

  return (
    <div className="min-h-screen bg-gray-50 flex flex-col">
      <MainPageHeader />

      <main className="flex-1 px-8 py-10">
        <div className="max-w-4xl mx-auto bg-white p-8 rounded-2xl shadow-xl">
          <h1 className="text-2xl font-semibold mb-8 text-center">Management Dashboard</h1>

          <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-6">
            {sections.map((section) => (
              <div
                key={section.title}
                onClick={() => !section.comingSoon && navigate(section.route)}
                className={`cursor-pointer border border-gray-300 rounded-xl p-6 shadow transition 
                  flex flex-col justify-center items-center text-center
                  ${section.comingSoon
                    ? "bg-gray-100 text-gray-500"
                    : "bg-white hover:bg-blue-600 hover:text-white hover:border-blue-600"
                  }`}
              >
                <h2 className="text-lg font-medium">{section.title}</h2>
                {section.comingSoon && (
                  <p className="text-sm mt-2 italic text-gray-400">Coming Soon</p>
                )}
              </div>
            ))}
          </div>
        </div>
      </main>

      <Footer />
    </div>
  );
}

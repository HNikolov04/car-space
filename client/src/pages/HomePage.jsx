import { Link } from "react-router-dom";
import { scroller } from "react-scroll";
import Header from "../components/shared/MainPageHeader";
import Footer from "../components/shared/Footer";

import heroCar from "../assets/hero-car.jpg";
import meetsImg from "../assets/meets.jpg";
import forumImg from "../assets/forum.jpg";
import servicesImg from "../assets/services.jpg";
import carsForSaleImg from "../assets/cars-for-sale.jpg";

import { ROUTES } from "../utils/routes/routes";

export default function HomePage() {
  const scrollToExplore = () => {
    scroller.scrollTo("explore", {
      duration: 800,
      delay: 0,
      smooth: "easeInOutQuart",
    });
  };

  return (
    <div className="min-h-screen flex flex-col bg-gray-50">
      <div className="relative z-50">
        <Header />
      </div>

      <main className="flex-1 w-full">
        <section className="relative w-full h-screen z-0">
          <img
            src={heroCar}
            alt="Car Meet"
            className="absolute inset-0 w-full h-full object-cover brightness-75 pointer-events-none z-0"
          />
          <div className="relative z-10 flex flex-col items-center justify-center h-full text-center text-white px-4">
            <h1 className="text-5xl font-bold mb-6">Welcome to CarSpace</h1>
            <p className="text-xl mb-8">
              Your hub for car meets, forums, services, and great cars for sale.
            </p>
            <button
              onClick={scrollToExplore}
              className="bg-blue-600 cursor-pointer hover:bg-blue-700 text-white font-semibold py-3 px-6 rounded-xl shadow-lg transition"
            >
              Get Started
            </button>
          </div>
        </section>

        <div name="explore" />

        <Section
          image={meetsImg}
          title="Find or Host Meets"
          description="Connect with local car enthusiasts and explore meetups near you."
          to={ROUTES.carMeet}
        />

        <Section
          image={forumImg}
          title="Join the Forum"
          description="Discuss builds, advice, and everything car-related with fellow petrolheads."
          to={ROUTES.carForum}
          flip
        />

        <Section
          image={servicesImg}
          title="Discover Car Services"
          description="Find recommended shops, detailing, tuning and performance upgrades."
          to={ROUTES.carService}
        />

        <Section
          image={carsForSaleImg}
          title="Buy or Sell Cars"
          description="Browse listings or post your car for sale today!"
          to={ROUTES.carShop}
          flip
        />
      </main>

      <Footer />
    </div>
  );
}

function Section({ image, title, description, to, flip = false }) {
  return (
    <section
      className={`flex flex-col md:flex-row ${
        flip ? "md:flex-row-reverse" : ""
      } items-center justify-between px-4 py-16 max-w-7xl mx-auto gap-8`}
    >
      <img
        src={image}
        alt={title}
        className="w-full md:w-1/2 rounded-2xl shadow-lg object-cover max-h-[400px]"
      />
      <div
        className={`w-full md:w-1/2 ${
          flip ? "md:text-right" : "md:text-left"
        } text-center`}
      >
        <h2 className="text-4xl font-bold mb-4">{title}</h2>
        <p className="text-lg text-gray-700 mb-6">{description}</p>
        <Link
          to={to}
          className="inline-block text-base font-semibold text-black bg-white hover:bg-blue-600 hover:text-white px-6 py-3 rounded-md shadow transition border border-gray-300"
        >
          Learn More
        </Link>
      </div>
    </section>
  );
}

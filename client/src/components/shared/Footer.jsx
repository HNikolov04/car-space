import { Link } from "react-router-dom";

export default function Footer() {
  const footerLink =
    "text-sm text-gray-600 hover:text-blue-600 transition duration-200";

  return (
    <footer className="bg-white border-t border-gray-200 mt-16 w-full">
      <div className="max-w-7xl mx-auto px-6 py-12">
        <div className="grid grid-cols-2 sm:grid-cols-4 gap-8 text-center sm:text-left">
          <div>
            <h4 className="text-lg font-semibold text-gray-900 mb-4">CarSpace</h4>
            <ul className="space-y-2">
              <li><Link to="/" className={footerLink}>Home</Link></li>
              <li><Link to="/about" className={footerLink}>About Us</Link></li>
              <li><Link to="/contact" className={footerLink}>Contact</Link></li>
              <li><Link to="/faq" className={footerLink}>FAQ</Link></li>
            </ul>
          </div>

          <div>
            <h4 className="text-lg font-semibold text-gray-900 mb-4">Discover</h4>
            <ul className="space-y-2">
              <li><Link to="/carMeet" className={footerLink}>Car Meets</Link></li>
              <li><Link to="/carForum" className={footerLink}>Car Forum</Link></li>
              <li><Link to="/carService" className={footerLink}>Services</Link></li>
              <li><Link to="/carShop" className={footerLink}>Buy/Sell Cars</Link></li>
            </ul>
          </div>

          <div>
            <h4 className="text-lg font-semibold text-gray-900 mb-4">Support</h4>
            <ul className="space-y-2">
              <li><Link to="/coming-soon" className={footerLink}>Help Center</Link></li>
              <li><Link to="/coming-soon" className={footerLink}>Report Issue</Link></li>
              <li><Link to="/coming-soon" className={footerLink}>Feedback</Link></li>
            </ul>
          </div>

          <div>
            <h4 className="text-lg font-semibold text-gray-900 mb-4">Legal</h4>
            <ul className="space-y-2">
              <li><Link to="/coming-soon" className={footerLink}>Privacy Policy</Link></li>
              <li><Link to="/coming-soon" className={footerLink}>Terms of Use</Link></li>
              <li><Link to="/coming-soon" className={footerLink}>Cookies</Link></li>
            </ul>
          </div>
        </div>
      </div>

      <div className="border-t border-gray-300 w-full" />

      <div className="text-center py-6 bg-white">
        <p className="text-base font-medium text-black">
          © {new Date().getFullYear()} <span className="font-semibold">CarSpace</span>. All Rights Reserved.
        </p>
      </div>
    </footer>
  );
}

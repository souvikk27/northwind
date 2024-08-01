/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
        './Pages/**/*.cshtml',
        './Views/**/*.cshtml',
        "./node_modules/flowbite/**/*.js"
  ],
  theme: {
    extend: {},
  },
  plugins: [
        require('@tailwindcss/forms'),
        require('@tailwindcss/typography'),
        require('flowbite/plugin')
  ],
}


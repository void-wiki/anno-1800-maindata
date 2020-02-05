module.exports = {
  '*.{js,jsx,mjs,ts,tsx}': 'eslint',

  '*.css': 'stylelint',
  '*.scss': 'stylelint --syntax=scss',

  '*.{js,jsx,mjs,ts,tsx,json,yaml,css,scss,less,html,vue,graphql,gql,md}': 'prettier --write',

  'package.json': 'prettier-package-json --write',
};

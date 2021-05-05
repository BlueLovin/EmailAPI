const { createProxyMiddleware } = require('http-proxy-middleware');

module.exports = function(app) {
  const protocol = process.env.REACT_APP_SSL_ENABLED === '1' ? "https" : "http";
  const host = protocol + '://localhost:' + process.env.REACT_APP_EMAILAPI_PORT;
  app.use(
    '/api/mail/send',
    createProxyMiddleware({
      target: host,
      changeOrigin: true,
    })
  );
};
const winston = require("winston");
require("express-async-errors");

const logger = winston.createLogger({
  level: "info",
  format: winston.format.json(),
  defaultMeta: { service: "user-service" },
  transports: [
    //
    // - Write all logs with importance level of `error` or less to `error.log`
    // - Write all logs with importance level of `info` or less to `combined.log`
    //
    new winston.transports.File({
      filename: "./log/error.log",
      level: "error",
    }),
    new winston.transports.File({ filename: "./log/combined.log" }),
  ],
  exceptionHandlers: [
    new winston.transports.File({ filename: "./log/exceptions.log" }),
  ],
});

process.on("uncaughtException", (ex) => {
  logger.error(ex.message, ex);
  //process.exit(1);
});
//new winston.transports.File({ filename: "uncaughtExceptions.log" })

process.on("unhandledRejection", (ex) => {
  logger.error(ex.message, ex);
  //process.exit(1);
});

module.exports = logger;

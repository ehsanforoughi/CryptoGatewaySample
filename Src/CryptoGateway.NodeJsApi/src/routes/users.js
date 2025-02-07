process.env["NODE_CONFIG_DIR"] = "./src/config/";
const config = require("config");
const auth = require("../middleware/auth");
const jwt = require("jsonwebtoken");
const bcrypt = require("bcrypt");
const _ = require("lodash");
const { User, validate } = require("../models/user");
const express = require("express");
const router = express.Router();

router.get("/me", auth, async (req, res) => {
  const user = await User.findById(req.user._id).select("-password");
  res.send(user);
});

router.post("/", async (req, res) => {
  const { error } = validate(req.body);
  if (error) return res.status(400).send(error.details[0].message);

  let user = await User.findOne({ where: { email: req.body.email } });
  if (user) return res.status(400).send("User already registered.");

  const salt = await bcrypt.genSalt(10);
  const password = await bcrypt.hash(req.body.password, salt);
  const newUser = await User.create({
    name: req.body.name,
    email: req.body.email,
    password: password,
    salt: salt,
  });

  const token = generateAuthToken(newUser);
  res
    .header("x-auth-token", token)
    .send(_.pick(newUser, ["id", "name", "email"]));
});

function generateAuthToken(user) {
  const token = jwt.sign({ id: user.id }, config.get("jwtPrivateKey"));
  return token;
}

module.exports = router;

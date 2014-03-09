var nodemailer = require("nodemailer");

var smtpTransport = nodemailer.createTransport("SMTP",{
    service: "Gmail",
    auth: {
        user: "markus.linderback@gmail.com",
        pass: "jtk001jtk001"
    }
});

module.exports = {
    sendmail: function(sender, receiver, subject, message) {
        var mailOptions = {
            from: sender,
            to: receiver,
            subject: subject,
            text: "", // plaintext body
            html: message // html body
        };

        smtpTransport.sendMail(mailOptions, function(error, response) {
            if (error) {
                console.log(error);
            } else {
                console.log("Message sent: " + response.message);
            }
            smtpTransport.close();
        });
    }
};

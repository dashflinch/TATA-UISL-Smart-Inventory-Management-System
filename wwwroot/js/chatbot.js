document.addEventListener("DOMContentLoaded", function () {

const chatbotBtn = document.getElementById("chatbotButton");
const chatbot = document.getElementById("chatbotContainer");
const closeBtn = document.getElementById("closeChat");
const sendBtn = document.getElementById("sendMessage");
const input = document.getElementById("userMessage");
const messages = document.getElementById("chatMessages");

if (!chatbotBtn || !chatbot || !closeBtn || !sendBtn || !input || !messages) {
    console.error("Chatbot elements not found.");
    return;
}

chatbotBtn.addEventListener("click", () => {
    chatbot.style.display = "flex";

    input.focus();
});

closeBtn.addEventListener("click", () => {
    chatbot.style.display = "none";
});

input.addEventListener("keypress", function (e) {
    if (e.key === "Enter") {
        sendMessage();
    }
});

sendBtn.addEventListener("click", sendMessage);

document.addEventListener("click", function (e) {

    if (!e.target.classList.contains("quick-btn"))
        return;

    const text = e.target.textContent.replace(/[^\w\s]/g, "").trim();

    input.value = text;

    sendMessage();
});

function sendMessage() {

    const message = input.value.trim();

    if (message === "")
        return;

    addUserMessage(message);

    input.value = "";

    sendBtn.disabled = true;
    input.disabled = true;

    const typing = document.createElement("div");
    typing.className = "bot-message typing";
    typing.innerHTML = "🤖 Inventory Assistant is typing...";
    messages.appendChild(typing);

    scrollToBottom();

    fetch("/Chatbot/Ask", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            message: message
        })
    })
        .then(res => res.json())
        .then(data => {

            setTimeout(() => {

                typing.remove();

                addBotMessage(data.reply);

                sendBtn.disabled = false;
                input.disabled = false;

                input.focus();

            }, 900);

        })
        .catch(() => {

            typing.remove();

            addBotMessage("❌ Something went wrong.");

            sendBtn.disabled = false;
            input.disabled = false;
        });
}

function addUserMessage(text) {

    const div = document.createElement("div");

    div.className = "user-message";

    div.textContent = text;

    messages.appendChild(div);

    scrollToBottom();
}

function addBotMessage(text) {

    const div = document.createElement("div");

    div.className = "bot-message";

    div.innerHTML = text;

    messages.appendChild(div);

    scrollToBottom();
}

function scrollToBottom() {

    messages.scrollTop = messages.scrollHeight;

}
});
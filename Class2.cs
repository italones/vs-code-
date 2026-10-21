import asyncio
import logging

from aiogram import Bot, Dispatcher, F
from aiogram.filters import CommandStart
from aiogram.types import (
    Message,
    CallbackQuery,
    InlineKeyboardMarkup,
    InlineKeyboardButton,
    ReactionTypeEmoji,
)
from aiogram.enums import ParseMode
from aiogram.client.default import DefaultBotProperties

from config import BOT_TOKEN, CHANNEL_ID

logging.basicConfig(level=logging.INFO)

bot = Bot(
    token=BOT_TOKEN,
    default=DefaultBotProperties(parse_mode=ParseMode.HTML),
)
dp = Dispatcher()

# Набор эмодзи для реакций (должны быть в списке разрешённых Telegram)
REACTION_EMOJIS = ["👍", "❤️", "🔥", "🎉", "😁", "🤔", "👀"]


# ---------- 1. Публикация поста в канал с включёнными реакциями ----------
@dp.message(CommandStart())
async def cmd_start(message: Message):
    await message.answer(
        "Привет! Я умею:\n"
        "• Публиковать посты в канал с включёнными реакциями\n"
        "• Показывать клавиатуру для установки реакции\n\n"
        "Команды:\n"
        "/post — опубликовать тестовый пост\n"
        "/demo — отправить пост с кнопками реакций"
    )


@dp.message(F.text == "/post")
async def publish_post(message: Message):
    # ВАЖНО: Telegram сам включает базовые реакции (👍 и т.п.)
    # при отправке поста. Дополнительно можно передать список реакций
    # в самом API — но через aiogram это делается через сырой запрос.
    sent = await bot.send_message(
        chat_id=CHANNEL_ID,
        text="<b>Новый пост!</b>\n\nПоставьте реакцию — какая вам больше нравится 👇",
    )
    await message.answer(f"✅ Пост опубликован. ID: <code>{sent.message_id}</code>")


# ---------- 2. Демо: inline-кнопки для реакций ----------
# Внимание: это НЕ настоящие реакции Telegram, а кнопки-эмодзи.
# Настоящие реакции ставит пользователь, нажимая на пост в канале.
@dp.message(F.text == "/demo")
async def demo_reactions(message: Message):
    # Собираем клавиатуру из эмодзи
    keyboard = InlineKeyboardMarkup(
        inline_keyboard=[
            [
                InlineKeyboardButton(
                    text=emoji,
                    callback_data=f"reaction:{emoji}"
                )
                for emoji in REACTION_EMOJIS[:4]
            ],
            [
                InlineKeyboardButton(
                    text=emoji,
                    callback_data=f"reaction:{emoji}"
                )
                for emoji in REACTION_EMOJIS[4:]
            ],
        ]
    )

    await message.answer(
        "Нажмите на эмодзи, чтобы «отреагировать» на пост:",
        reply_markup=keyboard,
    )


@dp.callback_query(F.data.startswith("reaction:"))
async def handle_reaction(callback: CallbackQuery):
    emoji = callback.data.split(":", 1)[1]
    await callback.answer(f"Вы поставили {emoji}!", show_alert=False)
    await callback.message.edit_text(
        f"Реакция: {emoji}\n\nСпасибо! 😊"
    )


# ---------- 3. Установка НАСТОЯЩЕЙ реакции ботом на сообщение в канале ----------
# Telegram Bot API позволяет боту ставить реакции на сообщения.
# Метод: setMessageReaction
@dp.message(F.text.startswith("/react"))
async def set_real_reaction(message: Message):
    """
    Использование: /react <message_id>
    Бот поставит 👍 на указанное сообщение в канале.
    """
    parts = message.text.split(maxsplit=1)
    if len(parts) < 2 or not parts[1].strip().isdigit():
        await message.answer("Использование: <code>/react 123</code>")
        return

    msg_id = int(parts[1].strip())

    try:
        await bot.set_message_reaction(
            chat_id=CHANNEL_ID,
            message_id=msg_id,
            reaction=[ReactionTypeEmoji(emoji="👍")],
        )
        await message.answer(f"✅ Реакция 👍 поставлена на сообщение <code>{msg_id}</code>")
    except Exception as e:
        await message.answer(f"❌ Ошибка: <code>{e}</code>")


async def main():
    await dp.start_polling(bot)


if __name__ == "__main__":
    asyncio.run(main())import asyncio
import logging

from aiogram import Bot, Dispatcher, F
from aiogram.filters import CommandStart
from aiogram.types import (
    Message,
    CallbackQuery,
    InlineKeyboardMarkup,
    InlineKeyboardButton,
    ReactionTypeEmoji,
)
from aiogram.enums import ParseMode
from aiogram.client.default import DefaultBotProperties

from config import BOT_TOKEN, CHANNEL_ID

logging.basicConfig(level=logging.INFO)

bot = Bot(
    token=BOT_TOKEN,
    default=DefaultBotProperties(parse_mode=ParseMode.HTML),
)
dp = Dispatcher()

# Набор эмодзи для реакций (должны быть в списке разрешённых Telegram)
REACTION_EMOJIS = ["👍", "❤️", "🔥", "🎉", "😁", "🤔", "👀"]


# ---------- 1. Публикация поста в канал с включёнными реакциями ----------
@dp.message(CommandStart())
async def cmd_start(message: Message):
    await message.answer(
        "Привет! Я умею:\n"
        "• Публиковать посты в канал с включёнными реакциями\n"
        "• Показывать клавиатуру для установки реакции\n\n"
        "Команды:\n"
        "/post — опубликовать тестовый пост\n"
        "/demo — отправить пост с кнопками реакций"
    )


@dp.message(F.text == "/post")
async def publish_post(message: Message):
    # ВАЖНО: Telegram сам включает базовые реакции (👍 и т.п.)
    # при отправке поста. Дополнительно можно передать список реакций
    # в самом API — но через aiogram это делается через сырой запрос.
    sent = await bot.send_message(
        chat_id=CHANNEL_ID,
        text="<b>Новый пост!</b>\n\nПоставьте реакцию — какая вам больше нравится 👇",
    )
    await message.answer(f"✅ Пост опубликован. ID: <code>{sent.message_id}</code>")


# ---------- 2. Демо: inline-кнопки для реакций ----------
# Внимание: это НЕ настоящие реакции Telegram, а кнопки-эмодзи.
# Настоящие реакции ставит пользователь, нажимая на пост в канале.
@dp.message(F.text == "/demo")
async def demo_reactions(message: Message):
    # Собираем клавиатуру из эмодзи
    keyboard = InlineKeyboardMarkup(
        inline_keyboard=[
            [
                InlineKeyboardButton(
                    text=emoji,
                    callback_data=f"reaction:{emoji}"
                )
                for emoji in REACTION_EMOJIS[:4]
            ],
            [
                InlineKeyboardButton(
                    text=emoji,
                    callback_data=f"reaction:{emoji}"
                )
                for emoji in REACTION_EMOJIS[4:]
            ],
        ]
    )

    await message.answer(
        "Нажмите на эмодзи, чтобы «отреагировать» на пост:",
        reply_markup=keyboard,
    )


@dp.callback_query(F.data.startswith("reaction:"))
async def handle_reaction(callback: CallbackQuery):
    emoji = callback.data.split(":", 1)[1]
    await callback.answer(f"Вы поставили {emoji}!", show_alert=False)
    await callback.message.edit_text(
        f"Реакция: {emoji}\n\nСпасибо! 😊"
    )


# ---------- 3. Установка НАСТОЯЩЕЙ реакции ботом на сообщение в канале ----------
# Telegram Bot API позволяет боту ставить реакции на сообщения.
# Метод: setMessageReaction
@dp.message(F.text.startswith("/react"))
async def set_real_reaction(message: Message):
    """
    Использование: /react <message_id>
    Бот поставит 👍 на указанное сообщение в канале.
    """
    parts = message.text.split(maxsplit=1)
    if len(parts) < 2 or not parts[1].strip().isdigit():
        await message.answer("Использование: <code>/react 123</code>")
        return

    msg_id = int(parts[1].strip())

    try:
        await bot.set_message_reaction(
            chat_id=CHANNEL_ID,
            message_id=msg_id,
            reaction=[ReactionTypeEmoji(emoji="👍")],
        )
        await message.answer(f"✅ Реакция 👍 поставлена на сообщение <code>{msg_id}</code>")
    except Exception as e:
        await message.answer(f"❌ Ошибка: <code>{e}</code>")


async def main():
    await dp.start_polling(bot)


if __name__ == "__main__":
    asyncio.run(main())
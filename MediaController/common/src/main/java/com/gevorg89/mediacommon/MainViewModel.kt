package com.gevorg89.mediacommon

import android.graphics.Bitmap
import android.graphics.BitmapFactory
import android.util.Base64
import android.util.Log
import androidx.lifecycle.DefaultLifecycleObserver
import androidx.lifecycle.LifecycleOwner
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import io.socket.client.IO
import io.socket.client.Socket
import kotlinx.coroutines.Job
import kotlinx.coroutines.delay
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import kotlin.properties.Delegates

object Logger {
    fun log(key: String, data: Array<Any>) {
        Log.d(key, data.joinToString())
    }
}

enum class SocketEvent(val value: Int) {
    VolumeUp(0),
    Previous(1),
    PlayPause(2),
    Next(3),
    VolumeDown(4),
    MoveLeft(5),
    MoveTop(6),
    MoveRight(7),
    MoveBottom(8),
    Click(9),
}

enum class MoveDirection {
    Left, Top, Right, Bottom
}

const val EVENT_NAME = "SocketEvent"

class MainViewModel : ViewModel(), DefaultLifecycleObserver {

    private var socket by Delegates.notNull<Socket>()

    private val _image: MutableStateFlow<Bitmap?> = MutableStateFlow(null)
    val image = _image.asStateFlow()

    private var startPress = false
    private var job: Job? = null

    override fun onCreate(owner: LifecycleOwner) {
        super.onCreate(owner)
        socket = IO.socket("http://10.0.2.2:8080")
//        socket = IO.socket("http://192.168.1.33:8080")

        socket.on(Socket.EVENT_CONNECT) {
            Logger.log("EVENT_CONNECT", it)
            //socket.emit("input", "Hello from Android!!!")
        }

        socket.on(Socket.EVENT_DISCONNECT) {
            Logger.log("EVENT_DISCONNECT", it)
        };
        socket.on(Socket.EVENT_CONNECT_ERROR) {
            Logger.log("EVENT_CONNECT_ERROR", it)
        }

        socket.on("server") {
            val decodedString: ByteArray = Base64.decode(it[0].toString(), Base64.DEFAULT);
            val decodedByte = BitmapFactory.decodeByteArray(decodedString, 0, decodedString.size)
            _image.value = decodedByte
            Logger.log("input", it)
        }
    }

    override fun onResume(owner: LifecycleOwner) {
        super.onResume(owner)
        if (!socket.connected()) {
            socket.connect();
        }
    }

    override fun onPause(owner: LifecycleOwner) {
        super.onPause(owner)
        socket.disconnect();
    }

    private fun emit(event: SocketEvent) {
        socket.emit(EVENT_NAME, event.value)
    }

    fun volumeUp() {
        emit(SocketEvent.VolumeUp)
    }

    fun previous() {
        emit(SocketEvent.Previous)
    }

    fun playPause() {
        emit(SocketEvent.PlayPause)
    }

    fun next() {
        emit(SocketEvent.Next)
    }

    fun volumeDown() {
        emit(SocketEvent.VolumeDown)
    }

    fun moveLeft() {
        emit(SocketEvent.MoveLeft)
    }

    fun moveTop() {
        emit(SocketEvent.MoveTop)
    }

    fun moveRight() {
        emit(SocketEvent.MoveRight)
    }

    fun moveBottom() {
        emit(SocketEvent.MoveBottom)
    }

    fun onStartPress(moveDirection: MoveDirection) {
        startPress = true
        job?.cancel()
        job = viewModelScope.launch {
            while (startPress) {
                delay(50)
                when (moveDirection) {
                    MoveDirection.Left -> moveLeft()
                    MoveDirection.Top -> moveTop()
                    MoveDirection.Right -> moveRight()
                    MoveDirection.Bottom -> moveBottom()
                }

            }
        }
    }

    fun onEndPress() {
        startPress = false
    }

    fun onClick() {
        emit(SocketEvent.Click)
    }

    override fun onCleared() {
        socket.disconnect()
        super.onCleared()
    }


}
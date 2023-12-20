package com.gevorg89.mobile

import android.graphics.Bitmap
import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.gestures.detectTapGestures
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.BoxScope
import androidx.compose.foundation.layout.BoxWithConstraints
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.asImageBitmap
import androidx.compose.ui.input.pointer.pointerInput
import androidx.compose.ui.unit.dp
import com.gevorg89.mediacommon.MoveDirection
import kotlinx.coroutines.launch

@Composable
fun Controller(
    bitmap: Bitmap?,
    onClick: () -> Unit,
    onStart: (MoveDirection) -> Unit,
    onEnd: () -> Unit,
) {
    BoxWithConstraints(
        modifier = Modifier
            .background(color = Color(0xFFE4E3E3))
            .fillMaxSize()
    ) {
        val widthItem = this.maxWidth / 3
        val heightItem = this.maxHeight / 3
        MouseButton(
            modifier = Modifier
                .align(Alignment.CenterStart)
                .size(widthItem, heightItem),
            onStart = { onStart(MoveDirection.Left) },
            onEnd = onEnd
        ) {
            DotIcon(modifier = Modifier.fillMaxSize())
        }


        MouseButton(
            modifier = Modifier
                .align(Alignment.TopCenter)
                .size(widthItem, heightItem),
            onStart = { onStart(MoveDirection.Top) },
            onEnd = onEnd
        ) {
            DotIcon(modifier = Modifier.fillMaxSize())
        }

        MouseButton(
            modifier = Modifier
                .align(Alignment.CenterEnd)
                .size(widthItem, heightItem),
            onStart = { onStart(MoveDirection.Right) },
            onEnd = onEnd
        ) {
            DotIcon(modifier = Modifier.fillMaxSize())
        }

        MouseButton(
            modifier = Modifier
                .align(Alignment.BottomCenter)
                .size(widthItem, heightItem),
            onStart = { onStart(MoveDirection.Bottom) },
            onEnd = onEnd
        ) {
            DotIcon(modifier = Modifier.fillMaxSize())
        }

        Box(
            modifier = Modifier
                .background(shape = RoundedCornerShape(50), color = Color(0xFFB1B0B0))
                .align(Alignment.Center)
                .size(widthItem, heightItem)
                .clip(shape = RoundedCornerShape(50))
                .clickable(onClick = onClick)
        ) {
            Box(modifier = Modifier.fillMaxSize(), contentAlignment = Alignment.Center) {
                Text(text = "OK")
            }
        }
    }

    if (bitmap != null) {
        Image(
            modifier = Modifier.fillMaxSize(),
            bitmap = bitmap.asImageBitmap(),
            contentDescription = "contentDescription"
        )
    }
}

@Composable
fun MouseButton(
    modifier: Modifier = Modifier,
    onStart: () -> Unit,
    onEnd: () -> Unit,
    content: @Composable BoxScope.() -> Unit,
) {
    val scope = rememberCoroutineScope()
    Box(
        modifier = modifier
            .pointerInput(Unit) {
                detectTapGestures(onPress = {
                    scope.launch {
                        onStart()
                        tryAwaitRelease()
                        onEnd()
                    }
                })
            }
    ) {
        content()
    }
}

@Composable
fun DotIcon(modifier: Modifier = Modifier) {
    Box(modifier = modifier, contentAlignment = Alignment.Center) {
        Box(
            modifier = Modifier
                .size(6.dp)
                .background(color = Color.Gray, shape = RoundedCornerShape(50))
        )
    }
}
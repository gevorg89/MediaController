/* While this template provides a good starting point for using Wear Compose, you can always
 * take a look at https://github.com/android/wear-os-samples/tree/main/ComposeStarter and
 * https://github.com/android/wear-os-samples/tree/main/ComposeAdvanced to find the most up to date
 * changes to the libraries and their usages.
 */

package com.gevorg89.wear.presentation

import android.graphics.Bitmap
import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.viewModels
import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.gestures.detectTapGestures
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.BoxScope
import androidx.compose.foundation.layout.BoxWithConstraints
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.asImageBitmap
import androidx.compose.ui.input.pointer.pointerInput
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.tooling.preview.Devices
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.core.splashscreen.SplashScreen.Companion.installSplashScreen
import androidx.wear.compose.material.Button
import androidx.wear.compose.material.Icon
import androidx.wear.compose.material.Text
import com.gevorg89.mediacommon.MainViewModel
import com.gevorg89.mediacommon.MoveDirection
import com.gevorg89.wear.R
import kotlinx.coroutines.launch

class MainActivity : ComponentActivity() {
    private val mainViewModel: MainViewModel by viewModels()
    override fun onCreate(savedInstanceState: Bundle?) {
        installSplashScreen()

        super.onCreate(savedInstanceState)
        lifecycle.addObserver(mainViewModel)
        setTheme(android.R.style.Theme_DeviceDefault)

        setContent {
//            MediaController(
//                volumeUp = { mainViewModel.volumeUp() },
//                volumeDown = { mainViewModel.volumeDown() },
//                previous = { mainViewModel.previous() },
//                next = { mainViewModel.next() },
//                playPause = { mainViewModel.playPause() },
//            )
            val bitmap by mainViewModel.image.collectAsState()
            Controller(
                bitmap = bitmap,
                onClick = { mainViewModel.onClick() },
                onStart = { mainViewModel.onStartPress(it) },
                onEnd = { mainViewModel.onEndPress() })
        }
    }
}

@Preview(device = Devices.WEAR_OS_LARGE_ROUND, showSystemUi = true)
@Composable
fun MediaControllerPreview() {
    MediaController(
        volumeUp = {},
        volumeDown = {},
        previous = {},
        next = {},
        playPause = {}
    )
}

@Composable
fun MediaController(
    volumeUp: () -> Unit,
    volumeDown: () -> Unit,
    previous: () -> Unit,
    next: () -> Unit,
    playPause: () -> Unit,
) {
    Column(
        modifier = Modifier
            .padding(8.dp)
            .fillMaxSize(), verticalArrangement = Arrangement.SpaceBetween
    ) {
        Box(
            modifier = Modifier.fillMaxWidth(),
            contentAlignment = Alignment.Center
        ) {
            Button(onClick = volumeUp) {
                Icon(
                    painter = painterResource(id = R.drawable.baseline_volume_up_24),
                    contentDescription = null
                )
            }
        }

        Box(modifier = Modifier.fillMaxWidth()) {
            Button(
                modifier = Modifier.align(Alignment.TopStart),
                onClick = previous
            ) {
                Icon(
                    painter = painterResource(id = R.drawable.previous_9),
                    contentDescription = null
                )
            }

            Button(
                modifier = Modifier.align(Alignment.Center),
                onClick = playPause
            ) {
                Icon(
                    painter = painterResource(id = R.drawable.player_play_pause),
                    contentDescription = null
                )
            }


            Button(
                modifier = Modifier.align(Alignment.TopEnd),
                onClick = next
            ) {
                Icon(
                    painter = painterResource(id = R.drawable.next_21),
                    contentDescription = null
                )
            }
        }

        Box(
            modifier = Modifier.fillMaxWidth(),
            contentAlignment = Alignment.Center
        ) {
            Button(onClick = volumeDown) {
                Icon(
                    painter = painterResource(id = R.drawable.volume_down_2),
                    contentDescription = null
                )
            }
        }
    }
}


@Preview(device = Devices.WEAR_OS_LARGE_ROUND, showSystemUi = true)
@Composable
fun PreviewController() {
    Controller(bitmap = null, onStart = {}, onEnd = {}, onClick = {})
}

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
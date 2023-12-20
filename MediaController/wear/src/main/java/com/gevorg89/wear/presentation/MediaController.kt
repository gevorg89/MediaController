package com.gevorg89.wear.presentation

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.tooling.preview.Devices
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.wear.compose.material.Button
import androidx.wear.compose.material.Icon
import com.gevorg89.wear.R

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
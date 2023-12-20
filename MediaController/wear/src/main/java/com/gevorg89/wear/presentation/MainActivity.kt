/* While this template provides a good starting point for using Wear Compose, you can always
 * take a look at https://github.com/android/wear-os-samples/tree/main/ComposeStarter and
 * https://github.com/android/wear-os-samples/tree/main/ComposeAdvanced to find the most up to date
 * changes to the libraries and their usages.
 */

package com.gevorg89.wear.presentation

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.viewModels
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.core.splashscreen.SplashScreen.Companion.installSplashScreen
import com.gevorg89.mediacommon.MainViewModel

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
package com.gevorg89.mobile

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.viewModels
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.tooling.preview.Preview
import com.gevorg89.mediacommon.MainViewModel
import com.gevorg89.mobile.ui.theme.MediaControllerTheme

class MainActivity : ComponentActivity() {

    private val mainViewModel: MainViewModel by viewModels()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            MediaControllerTheme {
                // A surface container using the 'background' color from the theme
                Surface(
                    modifier = Modifier.fillMaxSize(),
                    color = MaterialTheme.colorScheme.background
                ) {
                    val bitmap by mainViewModel.image.collectAsState()
                    Controller(
                        bitmap = bitmap,
                        onClick = { mainViewModel.onClick() },
                        onStart = { mainViewModel.onStartPress(it) },
                        onEnd = { mainViewModel.onEndPress() })
                }
            }
        }
    }
}

package com.littlebyte.climb;

import android.content.SharedPreferences;
import android.os.Bundle;
import android.os.Handler;
import android.os.StrictMode;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.TextView;

import java.util.List;

import climb.ApiException;
import climb.models.*;
import climb.services.*;

public class MainActivity extends AppCompatActivity {
    private AccountApi accountApi;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        accountApi = new AccountApi();
        accountApi.getApiClient().setBasePath("http://192.168.1.11:45455");

        StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder()
                .permitAll().build();
        StrictMode.setThreadPolicy(policy);
    }

    public void onRegisterClick(View view) {
        final TextView mTextView = findViewById(R.id.textView);
        mTextView.setText("registering...");

        final EditText emailInput = findViewById(R.id.emailInput);
        final String email = emailInput.getText().toString();

        final EditText passwordInput = findViewById(R.id.passwordInput);
        final String password = passwordInput.getText().toString();

        final EditText confirmInput = findViewById(R.id.confirmInput);
        final String confirm = confirmInput.getText().toString();

        Handler handler = new Handler();
        handler.post(new Runnable() {
            @Override
            public void run() {
                try {
                    ApplicationUser applicationUser = accountApi.accountRegister(email, password, confirm);
                    mTextView.setText("Welcome " + applicationUser.getEmail());
                } catch (ApiException e) {
                    mTextView.setText("Couldn't register!");
                    e.printStackTrace();
                }
            }
        });
    }

    public void onLoginClick(View view) {
        final TextView mTextView = findViewById(R.id.textView);
        mTextView.setText("logging in...");

        final EditText emailInput = findViewById(R.id.emailInput);
        final String email = emailInput.getText().toString();

        final EditText passwordInput = findViewById(R.id.passwordInput);
        final String password = passwordInput.getText().toString();

        LoginResponse loginResponse = null;
        try {
            loginResponse = accountApi.accountLogIn(email, password, true);
        } catch (ApiException e) {
            mTextView.setText("Couldn't log in!");
            e.printStackTrace();
        }
        loginResponse.getToken();
        mTextView.setText("Logged In with token: " + loginResponse.getToken());
        SharedPreferences preferences = getSharedPreferences("user", MODE_PRIVATE);
        SharedPreferences.Editor editor = preferences.edit();
        editor.putString("jwt", loginResponse.getToken());
        editor.commit();
    }

    public void onTestClick(View view) {
        final TextView mTextView = findViewById(R.id.textView);

        Handler handler = new Handler();
        handler.post(new Runnable() {
            @Override
            public void run() {
                try {
                    final SeasonApi seasonApi = new SeasonApi();
                    seasonApi.getApiClient().setBasePath("http://192.168.1.11:45455");
                    final List<Set> sets = seasonApi.seasonSets(1);
                    for (int i = 0; i < sets.size(); i++) {
                        final Set set = sets.get(i);
                        Log.i("Climb", set.getPlayer1ID() + " v " + set.getPlayer2ID());
                    }

                    mTextView.setText("Found sets:" + sets.size());
                } catch (ApiException e) {
                    mTextView.setText("Couldn't test!");
                    e.printStackTrace();
                }
            }
        });
    }
}

import * as React from "react";
import { Alert } from "react-native";
import { useState } from "react";
import { useRoute } from "@react-navigation/native";
import {
    Image,
    StyleSheet,
    Pressable,
    View,
    Text,
    KeyboardAvoidingView,
    Platform,
    ScrollView,
} from "react-native";
import { StackNavigationProp } from "@react-navigation/stack";
import { useNavigation, ParamListBase } from "@react-navigation/native";
import BigTextField from "../components/input/BigTextField";
import PrimaryButton from "../components/button/PrimaryButton";
import { Color, FontFamily, FontSize, Sizes, Padding, Border } from "../../../GlobalStyles";
import { AppViewModel, AppViewModelContext } from "../../viewModels/AppViewModel";
import ActivityProgressBar from "../components/progress/ActivityProgressBar";

const CreateAccountEnterPhone: React.FC = () => {
    const route = useRoute();
    const { isLoginScreen } = (route.params as { isLoginScreen: boolean }) || false;
    const navigation = useNavigation<StackNavigationProp<ParamListBase>>();
    const [isLoading, setIsLoading] = useState(false);

    // Creating a handler for the email change to keep up to date email address
    const [phoneNumber, setPhoneNumber] = useState<string>("");
    const handlePhoneNumberChange = (text: string) => {
        // Formatting the phone entered and updating UserInfo with it
        const cleaned = ("" + text).replace(/\D/g, "");
        const length = cleaned.length;
        let formattedPhone = "";
        if (length == 0) {
            formattedPhone = cleaned;
        } else if (length < 3) {
            formattedPhone = `(${cleaned}`;
        } else if (length == 3) {
            formattedPhone = `(${cleaned}`;
        } else if (length < 7) {
            formattedPhone = `(${cleaned.slice(0, 3)}) ${cleaned.slice(3)}`;
        } else {
            formattedPhone = `(${cleaned.slice(0, 3)}) ${cleaned.slice(3, 6)}-${cleaned.slice(6, 10)}`;
        }
        setPhoneNumber(formattedPhone);
    };
    const appViewModel = React.useContext(AppViewModelContext) as AppViewModel;
    // Handling Continue button press by saving the request to issue a PIN
    const handleSubmit = async () => {
        // Check if phone is correct
        const phoneDigits = phoneNumber.replace(/\s|-|\(|\)/g, ""); // Trimming spaces, hyphens, and parentheses from phone number
        if (phoneDigits.length != 10) {
            Alert.alert("Please enter a valid phone number.");
        } else {
            setIsLoading(true);
            const response = await appViewModel.handleUserEnterPhoneNumber(`+1${phoneDigits}`);
            setIsLoading(false);
            if (!response.ok) {
                Alert.alert(
                    "There was an issue sending sms to your phone number. Please re-enter your phone number. If this continues to happen, please email us at hello@trycommonly.com. "
                );
            } else {
                navigation.navigate("CreateAccountVerify", {
                    phoneString: `${phoneNumber}`,
                    isLoginScreen: isLoginScreen,
                });
            }
        }
    };

    return (
        <View style={{ flex: 1 }}>
            <KeyboardAvoidingView
                style={{ flex: 1 }}
                behavior={Platform.select({ ios: "padding", android: "height" })}
                keyboardVerticalOffset={Platform.select({ ios: 0, android: 0 })}
            >
                <ScrollView
                    contentContainerStyle={
                        Platform.OS === "android"
                            ? styles.scrollViewContentContainer
                            : { flexGrow: 1, marginBottom: 20 }
                    }
                    keyboardShouldPersistTaps="handled"
                >
                    <View style={styles.createAccountEnterEmail}>
                        <View style={styles.topFrame}>
                            <View style={styles.backAndLogoFrame}>
                                <Pressable style={styles.back} onPress={() => navigation.goBack()}>
                                    <Image
                                        style={styles.iconLayout}
                                        resizeMode="cover"
                                        source={require("../../assets/back.png")}
                                    />
                                </Pressable>
                                <View style={styles.logocomponent}>
                                    <Image
                                        style={[styles.commonlyIcon012, styles.iconLayout]}
                                        resizeMode="cover"
                                        source={require("../../assets/commonly-icon.png")}
                                    />
                                </View>
                            </View>

                            {isLoginScreen ? (
                                <Text style={[styles.youreAboutTo, styles.bySigningUpFlexBox]}>
                                    {`Welcome back 🙂\n\nPlease enter your phone number to log in.`}
                                </Text>
                            ) : (
                                <Text style={[styles.youreAboutTo, styles.bySigningUpFlexBox]}>
                                    You’re about to make new friends. Let’s get started 🙂
                                </Text>
                            )}
                        </View>
                        <BigTextField
                            hint="Phone number"
                            property1BigTextFieldMarginTop={64}
                            property1BigTextFieldWidth={330}
                            property1BigTextFieldAlignSelf="unset"
                            property1BigTextFieldPosition="relative"
                            property1BigTextFieldMarginLeft="unset"
                            onChangeText={(text) => handlePhoneNumberChange(text)}
                            value={phoneNumber}
                            keyboardType="phone-pad"
                        />
                        <View style={[styles.bottomFrame, styles.bottomFrameSpaceBlock]}>
                            <PrimaryButton
                                buttonText="Continue"
                                primaryButtonPosition="unset"
                                primaryButtonPaddingHorizontal="unset"
                                primaryButtonWidth={330}
                                primaryButtonMarginLeft="unset"
                                primaryButtonBottom="unset"
                                primaryButtonLeft="unset"
                                primaryButtonRight="unset"
                                buttonFlex={1}
                                primaryButtonMarginTop="unset"
                                onPrimaryButtonPress={() => handleSubmit()}
                            />
                            <Text style={[styles.bySigningUp, styles.bottomFrameSpaceBlock]}>
                                {isLoginScreen
                                    ? `By logging in, you agree with our Terms of Service & Privacy Policy`
                                    : `By signing up, you agree with our Terms of Service & Privacy Policy`}
                            </Text>
                        </View>
                    </View>
                </ScrollView>
            </KeyboardAvoidingView>
            <ActivityProgressBar isLoading={isLoading} />
        </View>
    );
};

const styles = StyleSheet.create({
    iconLayout: {
        height: "100%",
        width: "100%",
    },
    bySigningUpFlexBox: {
        display: "flex",
        textAlign: "center",
        color: Color.hintColor,
        fontFamily: FontFamily.bodyText,
        letterSpacing: 0,
        justifyContent: "center",
    },
    bottomFrameSpaceBlock: {
        marginTop: 24,
        alignItems: "center",
        overflow: "hidden",
    },
    back: {
        width: 48,
        height: 26,
        paddingHorizontal: 11,
    },
    commonlyIcon012: {
        position: "absolute",
        top: "0%",
        right: "0%",
        bottom: "0%",
        left: "0%",
        maxWidth: "100%",
        maxHeight: "100%",
        overflow: "hidden",
    },
    logocomponent: {
        width: 74,
        height: 74,
        marginLeft: 75,
    },
    backAndLogoFrame: {
        width: 329,
        flexDirection: "row",
        alignItems: "center",
        overflow: "hidden",
    },
    youreAboutTo: {
        fontSize: FontSize.body_size,
        lineHeight: FontSize.body_size * Sizes.fontLineMultiplier,
        width: 354,
        marginTop: 32,
        alignItems: "center",
    },
    topFrame: {
        width: 393,
        paddingHorizontal: 0,
        justifyContent: "center",
        alignItems: "center",
        overflow: "hidden",
    },
    bySigningUp: {
        fontSize: FontSize.size_xs,
        lineHeight: FontSize.size_xs * Sizes.fontLineMultiplier,
        width: 225,
        display: "flex",
        textAlign: "center",
        color: Color.hintColor,
        fontFamily: FontFamily.bodyText,
        letterSpacing: 0,
        justifyContent: "center",
    },
    bottomFrame: {
        width: 361,
        justifyContent: "flex-end",
        flex: 1,
        marginBottom: 20,
    },
    createAccountEnterEmail: {
        borderRadius: Border.global_radius,
        backgroundColor: Color.white,
        borderColor: Color.colorWhitesmoke,
        alignItems: "center",
        overflow: "hidden",
        paddingVertical: Padding.vertical,
        paddingHorizontal: Padding.horizontal,
        height: "100%",
        width: "100%",
        flex: 1,
    },
    scrollViewContentContainer: {
        flexGrow: 1,
        paddingBottom: 16, // Adjust this value based on your needs
    },
});

export default CreateAccountEnterPhone;

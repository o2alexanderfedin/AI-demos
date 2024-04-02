import * as React from 'react';
import { render, fireEvent, waitFor } from '@testing-library/react-native';
import CreateAccountEnterPhone from './CreateAccountEnterPhone';
import { AppViewModelContext } from '../../viewModels/AppViewModel';
import { Alert } from 'react-native';

describe('CreateAccountEnterPhone component - handleSubmit tests', () => {
  const mockHandleUserEnterPhoneNumber = jest.fn();
  const mockNavigate = jest.fn();
  const mockGoBack = jest.fn();

  jest.mock('@react-navigation/native', () => ({
    ...jest.requireActual('@react-navigation/native'),
    useNavigation: () => ({
      navigate: mockNavigate,
      goBack: mockGoBack,
    }),
  }));

  const mockAppViewModel = {
    handleUserEnterPhoneNumber: mockHandleUserEnterPhoneNumber,
  };

  const setup = () => {
    const utils = render(
      <AppViewModelContext.Provider value={mockAppViewModel}>
        <CreateAccountEnterPhone />
      </AppViewModelContext.Provider>
    );
    const input = utils.getByPlaceholderText('Phone number');
    const continueButton = utils.getByText('Continue');
    return {
      input,
      continueButton,
      ...utils,
    };
  };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('calls handleUserEnterPhoneNumber with correct formatted number on valid input', async () => {
    const { input, continueButton } = setup();
    const validPhoneNumber = '(123) 456-7890';
    const expectedArgument = '+11234567890';
    fireEvent.changeText(input, validPhoneNumber);
    fireEvent.press(continueButton);
    await waitFor(() => { // Make sure to wait for async operations
      expect(mockHandleUserEnterPhoneNumber).toHaveBeenCalledWith(expectedArgument);
    });
  });

  it('does not proceed with handleSubmit on invalid input', async () => {
    const { input, continueButton } = setup();
    fireEvent.changeText(input, '(123) 456-789'); // Incomplete phone number
    fireEvent.press(continueButton);
    await waitFor(() => {
      expect(mockHandleUserEnterPhoneNumber).not.toHaveBeenCalled();
      expect(mockNavigate).not.toHaveBeenCalled();
    });
  });

  it('navigates to the verification screen on successful phone number submission', async () => {
    const { input, continueButton } = setup();
    mockHandleUserEnterPhoneNumber.mockResolvedValue({ ok: true });
    const validPhoneNumber = '(123) 456-7890';
    fireEvent.changeText(input, validPhoneNumber);
    fireEvent.press(continueButton);
    await waitFor(() => {
      expect(mockNavigate).toHaveBeenCalledWith("CreateAccountVerify", {
        phoneString: validPhoneNumber,
        isLoginScreen: expect.any(Boolean), // Additional parameter validation if needed
      });
    });
  });

  it('shows an alert on an unsuccessful phone number submission', async () => {
    const alertSpy = jest.spyOn(Alert, 'alert');
    const { input, continueButton } = setup();
    mockHandleUserEnterPhoneNumber.mockResolvedValue({ ok: false });
    fireEvent.changeText(input, '(123) 456-7890');
    fireEvent.press(continueButton);
    await waitFor(() => {
      expect(alertSpy).toHaveBeenCalledWith(expect.any(String));
    });
  });
});
